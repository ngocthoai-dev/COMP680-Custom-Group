﻿using Core.Business;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core
{
    public class DefinitionManager : IDefinitionManager
    {
        private readonly Dictionary<Type, IList> _typeToCollection = new Dictionary<Type, IList>();
        private readonly ILogger _logger;
        private readonly IDefinitionLoader _definitionLoader;

        public DefinitionManager(
            IDefinitionLoader definitionLoader,
            ILogger logger)
        {
            _definitionLoader = definitionLoader;
            _logger = logger;
        }

        public async UniTask<TDefinition> GetDefinition<TDefinition>(string id) where TDefinition : class, IGameDefinition
        {
            IList<TDefinition> defs = await LoadDefinitions<TDefinition>();
            foreach (var i in defs)
                if (id == i.Id)
                    return DeepClone(i);

            throw new DefinitionNotFound(id);
        }

        public async UniTask<IList<TDefinition>> GetDefinitions<TDefinition>(string[] ids)
            where TDefinition : class, IGameDefinition
        {
            TDefinition[] result = new TDefinition[ids.Length];
            for (int i = 0; i < ids.Length; i++)
                result[i] = await GetDefinition<TDefinition>(ids[i]);

            return result;
        }

        public async UniTask<IList<TDefinition>> GetAllDefinition<TDefinition>() where TDefinition : class, IGameDefinition
        {
            IList<TDefinition> defs = await LoadDefinitions<TDefinition>();
            return defs;
        }

        public async UniTask<IList<TDefinition>> LoadDefinitions<TDefinition>() where TDefinition : class, IGameDefinition
        {
            IList list;
            Type type = typeof(TDefinition);
            if (_typeToCollection.TryGetValue(type, out list))
                return await TryToGetAlreadyLoadedOrWaitForDefinition<TDefinition>(list);
            else
                return await LoadNewDefinitions<TDefinition>();
        }

        private async UniTask<IList<TDefinition>> TryToGetAlreadyLoadedOrWaitForDefinition<TDefinition>(IList list)
            where TDefinition : class, IGameDefinition
        {
            if (list != null)
                return (IList<TDefinition>)list;
            else
                return await WaitToGetExistScript<TDefinition>();
        }

        private async UniTask<IList<TDefinition>> LoadNewDefinitions<TDefinition>() where TDefinition : class, IGameDefinition
        {
            IList list;
            Type type = typeof(TDefinition);
            _typeToCollection.Add(type, null);
            list = await _definitionLoader.LoadDefinitions<TDefinition>();
            _typeToCollection[type] = list;

            return (IList<TDefinition>)list;
        }

        private async UniTask<IList<TDefinition>> WaitToGetExistScript<TDefinition>()
        {
            int totalWaitms = 1000;
            int waitMs = 10;
            Type type = typeof(TDefinition);
            for (int i = 0; i < totalWaitms / waitMs; i++)
            {
                await UniTask.Delay(waitMs);
                if (_typeToCollection[type] != null)
                    return (IList<TDefinition>)_typeToCollection[type];
            }

            throw new WaitingAlreadyLoadedDefinitionTimeOut(type.ToString());
        }

        private static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        private class DefinitionNotFound : Exception
        {
            public DefinitionNotFound(string mess) : base(mess)
            {
            }
        }

        private class WaitingAlreadyLoadedDefinitionTimeOut : Exception
        {
            public WaitingAlreadyLoadedDefinitionTimeOut(string message) : base(message)
            {
            }
        }
    }
}
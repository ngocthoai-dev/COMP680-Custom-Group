using Core.Business;
using System;
using UnityEngine;

namespace Core.SO
{
    [Serializable]
    public abstract class BaseItemSO : IGameDefinition
    {
        [SerializeField] private string _id;

        public string Id
        { get => _id; set => _id = value; }

        public string Name;
    }
}
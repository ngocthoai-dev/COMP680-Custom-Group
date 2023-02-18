﻿using Core.EventSignal;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Zenject;

namespace Core
{
    public class CheckDownloadSize
    {
        private readonly SignalBus _signalBus;
        private double _totalCapacity;
        private string _errorMessage;
        private bool _error;

        public CheckDownloadSize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Init(string errorMessage)
        {
            _error = false;
            _totalCapacity = 0.0f;
            _errorMessage = errorMessage;
        }

        public async UniTaskVoid CheckSingleKey(object key, string errorMessage)
        {
            Init(errorMessage);
            await GetDownloadSizeAsync(key);
            CheckDone();
        }

        public async UniTaskVoid CheckMultipleKeys(List<string> keys, string errorMessage)
        {
            Init(errorMessage);
            await GetDownloadSizeAsync(keys);
            CheckDone();
        }

        public async UniTask<IList<IResourceLocation>> CheckMultipleKeys(
            List<string> keys,
            Addressables.MergeMode mergeMode,
            string errorMessage)
        {
            Init(errorMessage);
            var locations = await GetDownloadSizeAsync(keys, mergeMode);
            CheckDone();

            return locations;
        }

        private async UniTask GetDownloadSizeAsync(object key)
        {
            var handle = Addressables.GetDownloadSizeAsync(key);

            if (!handle.GetDownloadStatus().IsDone)
                await UniTask.NextFrame();

            SharedMethod_GetDownloadSizeAsync(handle);
        }

        private async UniTask GetDownloadSizeAsync(List<string> keys)
        {
            var handle = Addressables.GetDownloadSizeAsync(keys);

            if (!handle.GetDownloadStatus().IsDone)
                await UniTask.NextFrame();

            SharedMethod_GetDownloadSizeAsync(handle);
        }

        private async UniTask<IList<IResourceLocation>> GetDownloadSizeAsync(
            List<string> keys,
            Addressables.MergeMode mergeMode = Addressables.MergeMode.None)
        {
            // WARNING: Use "LoadResourceLocationsAsync" with "IResourceLocation" will
            // load each locations twice, 1st for the sprite, 2nd for the texture.
            var handle = Addressables.LoadResourceLocationsAsync(keys, mergeMode);

            if (!handle.GetDownloadStatus().IsDone)
                await UniTask.NextFrame();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // In case we want to see clearly which locations are downloaded
                // to see locations are duplicated between texture and sprite.
                //foreach (var location in handle.Result)
                //    UnityEngine.Debug.Log(location.PrimaryKey);

                var downloadHandle = Addressables.GetDownloadSizeAsync(handle.Result);

                if (!downloadHandle.GetDownloadStatus().IsDone)
                    await UniTask.NextFrame();

                SharedMethod_GetDownloadSizeAsync(downloadHandle);
                return handle.Result;
            }
            else if (
                handle.Status == AsyncOperationStatus.Failed ||
                handle.Status == AsyncOperationStatus.None)
            {
                _error = true;
            }

            Addressables.Release(handle);
            return null;
        }

        private void SharedMethod_GetDownloadSizeAsync(AsyncOperationHandle<long> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _totalCapacity = handle.Result / AddressableLoader.ONE_MEGABYTE_TO_BYTE;
            }
            else if (
                handle.Status == AsyncOperationStatus.Failed ||
                handle.Status == AsyncOperationStatus.None)
            {
                _error = true;
            }

            Addressables.Release(handle);
        }

        private void CheckDone()
        {
            if (!_error)
            {
                // TODO: Prepare for download or something...

                _signalBus.Fire(new CheckDownloadSizeStatusSignal(_totalCapacity));
            }
            else
            {
                // TODO: Handle error...
                // Here is an example of what happens if we catch an error.

                var signal = new AddressableErrorSignal(_errorMessage);
                _signalBus.Fire(signal);
            }
        }
    }
}
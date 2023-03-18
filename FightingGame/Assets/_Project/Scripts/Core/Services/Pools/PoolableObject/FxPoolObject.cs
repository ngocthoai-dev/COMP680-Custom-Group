using Core.Business;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Core
{
    public class FxPoolObject : TickablePoolObject
    {
        private readonly SignalBus _signalBus;
        private float _lifeTimer = 0.0f;

        public FxPoolObject(
            [Inject(Id = "PoolObjectContainer")]
            Transform poolObjectContainer,
            [Inject(Id = BundleLoaderName.Addressable)]
            IBundleLoader bundle,
            [Inject(Id = PoolName.Object)]
            IPoolManager poolManager,
            SignalBus signalBus) : base(poolObjectContainer, bundle, poolManager)
        {
            _signalBus = signalBus;
        }

        public override void Reinitialize()
        { }

        public void SelfDespawnAfter(float seconds)
        {
            seconds = Mathf.Max(seconds, 0);
            _lifeTimer = seconds;
        }

        public void Setup(Vector3 spawnPoint, Vector3 target)
        {
            _modelObj.WrappedObj.transform.position = spawnPoint;
            transform.LookAt(target, transform.up);
            TriggerSFXIfAvailable();
        }

        public void SetupWithForward(Vector3 spawnPoint, Vector3 forward)
        {
            _modelObj.WrappedObj.transform.position = spawnPoint;
            transform.forward = forward;
            TriggerSFXIfAvailable();
        }

        public void Setup(Vector3 spawnPoint)
        {
            _modelObj.WrappedObj.transform.position = spawnPoint;
            TriggerSFXIfAvailable();
        }

        public override void SelfDespawn(IPoolManager poolManager)
        {
            poolManager.Despawn(this);
        }

        private void TriggerSFXIfAvailable()
        {
            if (_modelObj.WrappedObj.TryGetComponent<SFXTrigger>(out var sfxTrigger))
                sfxTrigger.Trigger(_signalBus);
        }

        public override void Tick()
        {
            if (_modelObj == null || !_modelObj.IsActive)
            {
                _poolManager.Despawn(this);
                return;
            }

            if (_lifeTimer > 0.0f)
            {
                _lifeTimer -= Time.deltaTime;
                if (_lifeTimer <= 0.0f)
                    _poolManager.Despawn(this);
            }
        }
    }
}
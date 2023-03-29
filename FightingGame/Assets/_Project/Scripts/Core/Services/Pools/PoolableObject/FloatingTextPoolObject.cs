using Core.Business;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core
{
    public class FloatingTextPoolObject : TickablePoolObject
    {
        private TextMeshProUGUI _text;
        private float _despawnTimer = -1.0f;
        private Vector3 _dest;

        public FloatingTextPoolObject(
            [Inject(Id = "PoolObjectContainer")]
            Transform poolObjectContainer,
            [Inject(Id = BundleLoaderName.Addressable)]
            IBundleLoader bundle,
            [Inject(Id = PoolName.Object)]
            IPoolManager poolManager) : base(poolObjectContainer, bundle, poolManager)
        {
        }

        public override void Reinitialize()
        { }

        public void SelfDespawnAfter(float seconds)
        {
            _despawnTimer = seconds;
        }

        public void Setup(string text, Vector3 spawnPoint)
        {
            _modelObj.WrappedObj.transform.position = spawnPoint;
            if (_text == null)
                _text = _modelObj.WrappedObj.GetComponentInChildren<TextMeshProUGUI>();
            _text.SetText(text);
            _dest = spawnPoint + Vector3.up * Random.Range(1f, 2f);
        }

        public override void Tick()
        {
            if (_modelObj == null || !_modelObj.IsActive)
            {
                _poolManager.Despawn(this);
                return;
            }

            if (_despawnTimer > 0.0f)
            {
                transform.position = Vector3.Lerp(transform.position, _dest, Time.deltaTime);
                _despawnTimer -= Time.deltaTime;
                if (_despawnTimer <= 0.0f)
                    _poolManager.Despawn(this);
            }
        }

        public override void SelfDespawn(IPoolManager poolManager)
        {
            poolManager.Despawn(this);
        }
    }
}
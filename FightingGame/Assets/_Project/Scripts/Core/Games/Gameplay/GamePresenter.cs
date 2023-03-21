using Core.Business;
using Core.Utility;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.Gameplay
{
    public class GamePresenter
    {
        private readonly IPoolManager _poolManager;

        VertexGradient _lostTextGradColor = new(
            new Color(1, 0, 0), new Color(1, 0.5f, 0), new Color(1, 0, 0.5f), new Color(0.5f, 0, 1));
        VertexGradient _healTextGradColor = new(
            new Color(0, 1, 0), new Color(1, 1, 0), new Color(0.5f, 1, 0.5f), new Color(0, 1, 1));
        VertexGradient _defaultTextGradColor = new();

        public GamePresenter(
            [Inject(Id = PoolName.Object)]
            IPoolManager poolManager)
        {
            _poolManager = poolManager;
        }

        public async UniTask<IPoolObject> SpawnFXObjectWithForward(Vector3 position, string prefabPath, float despawnTimer = -1.0f)
        {
            FxPoolObject obj = await _poolManager.GetObject<FxPoolObject>(prefabPath);
            obj.Setup(position);
            if (despawnTimer > 0)
                obj.SelfDespawnAfter(despawnTimer);
            return obj;
        }

        private async UniTask<IPoolObject> SpawnFloatingTextObject(string text, Vector3 spawnPos, float despawnTimer = -1.0f)
        {
            Vector3 randomPos = spawnPos + Random.insideUnitSphere * 0.5f;
            FloatingTextPoolObject obj = await _poolManager.GetObject<FloatingTextPoolObject>(Defines.PrefabKey.HP_FX);
            obj.Setup(text, randomPos);
            obj.SelfDespawnAfter(despawnTimer);
            return obj;
        }

        public async UniTask<IPoolObject> SpawnFloatingDamageTex(string text, Vector3 spawnPos, float despawnTimer = -1.0f)
        {
            BasePoolObject obj = (BasePoolObject)await SpawnFloatingTextObject(text, spawnPos, despawnTimer);
            VertexGradient color = _defaultTextGradColor;
            if (float.TryParse(text, out float damage))
                color = damage <= 0 ? _lostTextGradColor : _healTextGradColor;
            obj.transform.GetComponentInChildren<TextMeshProUGUI>().colorGradient = color;
            return obj;
        }
    }
}
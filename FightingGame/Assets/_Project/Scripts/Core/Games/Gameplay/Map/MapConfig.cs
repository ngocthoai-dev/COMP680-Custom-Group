using Core.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Core.Gameplay
{
    public class MapConfig : MonoBehaviour
    {
        [SerializeField][DebugOnly] private Transform _spawnPositionGO;
        public Bounds Bound => _bound.bounds;
        [SerializeField][DebugOnly] private BoxCollider2D _bound;

        [SerializeField][DebugOnly] private Transform[] _spawnPositions;
        [SerializeField][DebugOnly] private bool[] _occupiedSpawnPositions;
        public List<Transform> OccupiedPositions { get; set; } = new();

        private void Awake()
        {
            _bound = GetComponent<BoxCollider2D>();
            _spawnPositionGO = transform.Find("SpawnPositions");
            _spawnPositions = new Transform[_spawnPositionGO.childCount];
            _occupiedSpawnPositions = new bool[_spawnPositionGO.childCount];
            for (int idx = 0; idx < _spawnPositionGO.childCount; idx++)
                _spawnPositions[idx] = _spawnPositionGO.GetChild(idx);
            gameObject.SetActive(false);
        }

        private void CheckResetOccupiedSpawnPositions()
        {
            if (_occupiedSpawnPositions.All(ele => ele))
                _occupiedSpawnPositions = new bool[_occupiedSpawnPositions.Length];
        }

        public Transform GetRandomSpawnTransform()
        {
            int[] availIdxes = _occupiedSpawnPositions.Select((occupied, idx) => (occupied, idx))
                    .Where(pair => !pair.occupied).Select(pair => pair.idx).ToArray();
            int randomIdx = Random.Range(0, availIdxes.Length);
            _occupiedSpawnPositions[availIdxes[randomIdx]] = true;

            OccupiedPositions.Add(_spawnPositions[availIdxes[randomIdx]]);
            CheckResetOccupiedSpawnPositions();
            return OccupiedPositions.Last();
        }
    }
}
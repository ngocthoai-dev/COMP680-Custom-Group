using Core.SO;
using Core.Utility;
using Shared.Extension;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerAttackCollision : MonoBehaviour
    {
        [SerializeField][DebugOnly] private int _characterLayer;
        [SerializeField][DebugOnly] protected AttackContainer _attackContainer;
        [SerializeField] protected bool _isDestroyOnTrigger = false;

        [SerializeField][DebugOnly] List<Collider2D> _others = new();

        private void Awake()
        {
            if (!TryGetComponent(out _attackContainer))
                _attackContainer = GetComponentInParent<AttackContainer>();
            _characterLayer = LayerMask.NameToLayer("Character");
        }

        protected virtual void OnDisable()
        {
            _others.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_others.Contains(collision)) return;
            _others.Add(collision);
            if (_characterLayer != collision.gameObject.layer) return;
            if (!collision.TryGetComponent(out CharacterController2D other)) return;
            if (collision.gameObject.ReferenceEqual(_attackContainer.Controller.gameObject)) return;
            if (_attackContainer.Config == null) return;

            float dmg = _attackContainer.Controller.CalculateDmg((int)_attackContainer.AtkIndex, other.GetStatsValue(StatType.Arm));
            other.OnHit(dmg, _attackContainer.Config.KnockType);

            if (_isDestroyOnTrigger) gameObject.SetActive(false);
        }
    }
}
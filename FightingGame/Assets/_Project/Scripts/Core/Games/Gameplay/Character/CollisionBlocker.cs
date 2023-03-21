using Core.Utility;
using UnityEngine;

namespace Core.Gameplay
{
    public class CollisionBlocker : MonoBehaviour
    {
        [SerializeField][DebugOnly] private Collider2D _characterCollider;
        [SerializeField][DebugOnly] private Collider2D _characterBlockCollider;

        private void Awake()
        {
            _characterCollider = GetComponentInParent<CharacterController2D>().GetComponent<Collider2D>();
            _characterBlockCollider = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(_characterCollider, _characterBlockCollider);
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("AttackCollision"));
        }
    }
}
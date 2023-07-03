using UnityEngine;

namespace Environment
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _selfCollider;

        public Collider2D Collider => _selfCollider;
        
        public Projectile Setup(Vector2 direction)
        {
            _rigidbody.velocity = direction * _speed;
            return this;
        }

        public int Catch()
        {
            Destroy(gameObject);
            return _damage;
        }
    }
}
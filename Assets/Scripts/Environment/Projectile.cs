using Unity.Netcode;
using UnityEngine;

namespace Environment
{
    public class Projectile : NetworkBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _selfCollider;
        [SerializeField] private NetworkObject _networkObject;
        
        public Collider2D Collider => _selfCollider;
        
        public Projectile Setup(Vector2 direction)
        {
            _rigidbody.velocity = direction * _speed;
            _networkObject.Spawn(true);
            return this;
        }

        public int Catch()
        {
            _networkObject.Despawn(true);
            return _damage;
        }
    }
}
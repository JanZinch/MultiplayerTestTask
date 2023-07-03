using UnityEngine;

namespace Environment
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;
        
        public void Setup(Vector2 direction)
        {
            _rigidbody.velocity = direction * _speed;
        }

        public int Catch()
        {
            Destroy(gameObject);
            return _damage;
        }
    }
}
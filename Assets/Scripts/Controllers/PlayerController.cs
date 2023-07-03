using System;
using Environment;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed = 3.0f;
        [SerializeField] private int _damage = 35;
        [SerializeField] private float _shootingCooldown = 1.0f;
        
        [Space]
        [SerializeField] private Joystick _motionJoystick;
        [SerializeField] private Button _shootingButton;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private DestructibleObject _destructible;

        private Vector2 _motion = default;
        
        private static readonly Vector3 PlayerRotationMask = new Vector3(0.0f, 0.0f, 1.0f);
        
        private void MoveByJoystick()
        {
            Debug.Log("Direction: " + _motionJoystick.Direction);
            
            _motion = new Vector2(
                _motionJoystick.Horizontal * _maxSpeed,
                _motionJoystick.Vertical * _maxSpeed);

            _rigidbody.velocity = _motion;
            
            if (_motion != Vector2.zero)
            {
                transform.LookAt2D((Vector2)transform.position + _motion.normalized);
            }
            
        }
        

        private void Update()
        {
            MoveByJoystick();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Coin>(out Coin coin))
            {
                coin.Collect();
            }
            else if (other.TryGetComponent<Projectile>(out Projectile projectile))
            {
                _destructible.MakeDamage(projectile.Catch());
            }
        }
    }
}
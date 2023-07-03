using System.Collections.Generic;
using Common;
using Environment;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed = 3.0f;
        [SerializeField] private float _shootingCooldown = 1.0f;
        
        [Space]
        [SerializeField] private Joystick _motionJoystick;
        [SerializeField] private Button _shootingButton;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private DestructibleObject _destructible;
        [SerializeField] private Score _score;
        [SerializeField] private List<Transform> _projectileSpawnPoints;
        [SerializeField] private Collider2D _selfCollider;
        
        private float _timeBetweenShots;
        private Vector2 _motion;
        
        private void OnEnable()
        {
            _shootingButton.onClick.AddListener(ShootIfReady);
        }

        private void MoveByJoystick()
        {
            _motion = _motionJoystick.Direction * _maxSpeed;
            _rigidbody.velocity = _motion;
            
            if (_motion != Vector2.zero)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, _motionJoystick.Direction);
            }
        }

        private void ShootIfReady()
        {
            if (_timeBetweenShots > _shootingCooldown)
            {
                foreach (Transform point in _projectileSpawnPoints)
                {
                    Projectile projectile = EnvironmentSpawner.Instance.SpawnProjectile(point.position, transform.up);
                    Physics2D.IgnoreCollision(_selfCollider, projectile.Collider);
                }
                
                _timeBetweenShots = 0.0f;
            }
        }
        
        private void Update()
        {
            _timeBetweenShots += Time.deltaTime;
            
            MoveByJoystick();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Coin>(out Coin coin))
            {
                _score.Add(coin.Collect());
            }
            else if (other.TryGetComponent<Projectile>(out Projectile projectile))
            {
                _destructible.MakeDamage(projectile.Catch());
            }
        }
        
        private void OnDisable()
        {
            _shootingButton.onClick.RemoveListener(ShootIfReady);
        }
    }
}
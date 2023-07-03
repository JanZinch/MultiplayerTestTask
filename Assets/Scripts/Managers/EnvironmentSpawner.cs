using System;
using System.Collections;
using Environment;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class EnvironmentSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnCooldown = 5f;
        [SerializeField] private Rect _coinsSpawnArea = new Rect(0f, 0f, 16f, 8f);
        
        [Header("Originals")]
        [SerializeField] private Projectile _projectileOriginal;
        [SerializeField] private Coin _coinOriginal;
        
        public static EnvironmentSpawner Instance { private set; get; }
        
        private Vector3 _cachedCoinSpawnPosition;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            StartCoroutine(SpawnCoins(_spawnCooldown));
        }

        public Projectile SpawnProjectile(Vector2 position, Vector2 direction)
        {
            return Instantiate<Projectile>(_projectileOriginal, position, 
                Quaternion.LookRotation(Vector3.forward, direction)).Setup(direction);
        }
        
        public Coin SpawnCoin(Vector2 position)
        {
            return Instantiate<Coin>(_coinOriginal, position, Quaternion.identity);
        }

        private IEnumerator SpawnCoins(float spawnCooldown)
        {
            WaitForSeconds spawnCooldownWait = new WaitForSeconds(spawnCooldown);
            
            while (true)
            {
                _cachedCoinSpawnPosition.x = Random.Range(
                    _coinsSpawnArea.x - _coinsSpawnArea.width / 2.0f,
                    _coinsSpawnArea.x + _coinsSpawnArea.width / 2.0f);
            
                _cachedCoinSpawnPosition.y = Random.Range(
                    _coinsSpawnArea.y - _coinsSpawnArea.height / 2.0f,
                    _coinsSpawnArea.y + _coinsSpawnArea.height / 2.0f);

                SpawnCoin(_cachedCoinSpawnPosition);
            
                yield return spawnCooldownWait;
            }
        }

    }
}
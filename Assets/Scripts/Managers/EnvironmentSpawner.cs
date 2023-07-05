using System;
using System.Collections;
using Environment;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class EnvironmentSpawner : NetworkBehaviour
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

        public override void OnNetworkSpawn()
        {
            Debug.Log("O IsHost: " + IsHost);
        }

        private void Start()
        {
            Debug.Log("S IsHost: " + IsHost);
            
            if (IsHost)
            {
                StartCoroutine(SpawnCoins(_spawnCooldown));
            }
        }

        public Projectile SpawnProjectile(Vector2 position, Vector2 direction)
        {
            return Instantiate<Projectile>(_projectileOriginal, position, 
                Quaternion.LookRotation(Vector3.forward, direction)).Setup(direction);
        }
        
        public Coin SpawnCoin(Vector2 position)
        {
            return Instantiate<Coin>(_coinOriginal, position, Quaternion.identity).SynchronizeInNetwork();
        }

        private IEnumerator SpawnCoins(float spawnCooldown)
        {
            WaitForSeconds spawnCooldownWait = new WaitForSeconds(spawnCooldown);
            
            while (true)
            {
                _cachedCoinSpawnPosition.x = Random.Range(_coinsSpawnArea.xMin, _coinsSpawnArea.xMax);
                _cachedCoinSpawnPosition.y = Random.Range(_coinsSpawnArea.yMin, _coinsSpawnArea.yMax);

                SpawnCoin(_cachedCoinSpawnPosition);
            
                yield return spawnCooldownWait;
            }
        }

    }
}
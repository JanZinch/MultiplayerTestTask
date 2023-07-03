using Environment;
using UnityEngine;

namespace Managers
{
    public class EnvironmentSpawner : MonoBehaviour
    {
        [SerializeField] private Projectile _projectileOriginal;

        public static EnvironmentSpawner Instance { private set; get; }

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

        public Projectile SpawnProjectile(Vector2 position, Vector2 direction)
        {
            return Instantiate<Projectile>(_projectileOriginal, position, 
                Quaternion.LookRotation(Vector3.forward, direction)).Setup(direction);
        }

    }
}
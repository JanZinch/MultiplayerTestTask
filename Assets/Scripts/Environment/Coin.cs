using UnityEngine;

namespace Environment
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int _price = 1;
        
        public int Collect()
        {
            Destroy(gameObject);
            return _price;
        }
    }
}
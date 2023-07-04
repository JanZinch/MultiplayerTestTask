using Unity.Netcode;
using UnityEngine;

namespace Environment
{
    public class Coin : NetworkBehaviour
    {
        [SerializeField] private int _price = 1;
        [SerializeField] private NetworkObject _networkObject;

        public Coin SynchronizeInNetwork()
        {
            _networkObject.Spawn(true);
            return this;
        }

        public int Collect()
        {
            if (IsHost)
            {
                _networkObject.Despawn(true);
            }
            
            return _price;
        }
    }
}
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "room_config", menuName = "Application/RoomConfig", order = 0)]
    public class RoomConfig : ScriptableObject
    {
        [SerializeField] private Rect _area;

        public Rect Area => _area;
    }
}
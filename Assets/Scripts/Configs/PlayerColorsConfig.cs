using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "player_colors", menuName = "Application/PlayerColorsConfig", order = 0)]
    public class PlayerColorsConfig : ScriptableObject
    {
        [SerializeField] private List<Color> _playerColors;
        
        public Color GetRandomColor()
        {
            return _playerColors[Random.Range(0, _playerColors.Count)];
        }
    }
}
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private TextMeshProUGUI _coinsCounter;
        [SerializeField] private Follower2D _follower;

        public Slider HealthBar => _healthBar;
        public TextMeshProUGUI CoinsCounter => _coinsCounter;

        public void SetPlayerName(string playerName)
        {
            _nameText.text = playerName;
        }

        public void SetTargetToFollow(Transform target)
        {
            _follower.SetTarget(target);
        }
    }
}
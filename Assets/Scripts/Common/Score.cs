using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Common
{
    public class Score : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private NetworkVariable<int> _score = new NetworkVariable<int>(0);

        public override void OnNetworkSpawn()
        {
            _score.OnValueChanged += OnValueChanged;
        }

        private void OnValueChanged(int previous, int current)
        {
            if (_scoreText != null) _scoreText.text = current.ToString();
        }

        public void SetView(TextMeshProUGUI text)
        {
            _scoreText = text;
            Set(0);
        }

        public void Set(int score)
        {
            _score.Value = score;
        }

        public void Add(int addend)
        {
            Set(_score.Value + addend);
        }

        public int Get()
        {
            return _score.Value;
        }
    }
}
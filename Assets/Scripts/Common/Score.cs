using System;
using TMPro;
using UnityEngine;

namespace Common
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private int _score;

        private void Awake()
        {
            Set(0);
        }

        public void Set(int score)
        {
            _score = score;
            _scoreText.text = _score.ToString();
        }

        public void Add(int addend)
        {
            Set(_score + addend);
        }

        public int Get()
        {
            return _score;
        }
    }
}
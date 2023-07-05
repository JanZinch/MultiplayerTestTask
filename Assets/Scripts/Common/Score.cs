using System;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Common
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private int _score;

        /*public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            Debug.Log("SET_");
            Set(0);

            Debug.Log("view: " + (_scoreText != null));
        }*/

        public void SetView(TextMeshProUGUI text)
        {
            _scoreText = text;
            Set(0);
        }

        public void Set(int score)
        {
            _score = score;
            if (_scoreText != null) _scoreText.text = _score.ToString();
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
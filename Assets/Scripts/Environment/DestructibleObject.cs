using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Environment
{
    public class DestructibleObject : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private UIBehaviour _view;
        [SerializeField] private UnityEvent<int> _onHealthUpdate;
        [SerializeField] private UnityEvent _onDeath;
        
        private int _health;
        
        public double Health => _health;
        public UnityEvent<int> OnHealthUpdate => _onHealthUpdate;
        public UnityEvent OnDeath => _onDeath;
        
        private void Awake()
        {
            _health = _maxHealth;
            UpdateView(true);
        }

        public void MakeDamage(int damage)
        {
            _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
            
            _onHealthUpdate?.Invoke(_health);
            
            if (_health <= 0)
            {
                _onDeath?.Invoke();
                
                _onHealthUpdate?.RemoveAllListeners();
                _onDeath?.RemoveAllListeners();
            }
            
            UpdateView();
        }

        public void SetView(UIBehaviour view)
        {
            _view = view;
        }

        private void UpdateView(bool init = false)
        {
            if (_view == null)
            {
                return;
            }

            if (_view is Slider slider)
            {
                if (init)
                {
                    slider.maxValue = _maxHealth;
                }

                slider.value = _health;
            }
            else if (_view is TextMeshProUGUI textMesh)
            {
                textMesh.text = string.Format("{0:d}/{1:d}", _health, _maxHealth);
            }
        }

    }
}
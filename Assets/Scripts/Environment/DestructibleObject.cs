using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Environment
{
    public class DestructibleObject : NetworkBehaviour
    {
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private UIBehaviour _view;
        [SerializeField] private UnityEvent<int> _onHealthUpdate;
        [SerializeField] private UnityEvent _onDeath;
        
        private NetworkVariable<int> _health = new NetworkVariable<int>();
        
        public double Health => _health.Value;
        public UnityEvent<int> OnHealthUpdate => _onHealthUpdate;
        public UnityEvent OnDeath => _onDeath;

        public override void OnNetworkSpawn()
        {
            _health.OnValueChanged += OnHealthValueChanged;
            _health.Value = _maxHealth;
        }

        private void OnHealthValueChanged(int previous, int current)
        {
            UpdateView();
            
            _onHealthUpdate?.Invoke(_health.Value);
            
            if (_health.Value <= 0)
            {
                _onDeath?.Invoke();
                
                _onHealthUpdate?.RemoveAllListeners();
                _onDeath?.RemoveAllListeners();
            }
        }
        
        public void MakeDamage(int damage)
        {
            _health.Value = Mathf.Clamp(_health.Value - damage, 0, _maxHealth);
        }

        public void SetView(UIBehaviour view)
        {
            _view = view;
        }

        private void UpdateView()
        {
            if (_view == null)
            {
                return;
            }

            if (_view is Slider slider)
            {
                slider.maxValue = _maxHealth;
                slider.value = _health.Value;
            }
            else if (_view is TextMeshProUGUI textMesh)
            {
                textMesh.text = string.Format("{0:d}/{1:d}", _health.Value, _maxHealth);
            }
        }

    }
}
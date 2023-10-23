using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.HudButtons
{
    [RequireComponent(typeof(Button))]
    public class MainHudButton : MonoBehaviour
    {
        [SerializeField] protected WindowType _targetWindowType;
        [SerializeField] private Button _button;

        public event Action<WindowType> OnButtonClicked;

        public void Init()
        {
            if (_button == null)
            {
                gameObject.SetActive(false);
                Debug.Log("MainHudButton || Button not set to work with");
                return;
            }
            
            _button.onClick.AddListener(OnButtonClick);
        }

        protected virtual void OnValidate()
        {
            if (TryGetComponent(out Button button))
            {
                _button = button;
            }
        }

        protected virtual void OnButtonClick()
        {
            OnButtonClicked?.Invoke(_targetWindowType);
        }
    }
}
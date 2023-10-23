using UnityEngine;

namespace Ui.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField] protected WindowType _windowType;
        [SerializeField] protected GameObject _windowContainer;
        [Space] 
        [SerializeField] private bool _isWindowCloseOnStart = true;

        public WindowType WindowType => _windowType;
        public bool IsWindowOpened { get; protected set; } = false;
        
        public virtual void Init()
        {
            gameObject.SetActive(!_isWindowCloseOnStart);
            IsWindowOpened = !_isWindowCloseOnStart;
        }

        public virtual void Show()
        {
            if (IsWindowOpened) return;
            
            gameObject.SetActive(true);
            IsWindowOpened = true;
        }

        public virtual void Hide()
        {
            if (!IsWindowOpened) return;
            
            gameObject.SetActive(false);
            IsWindowOpened = false;
        }

        protected virtual void OnValidate()
        {
            
        }
    }
}
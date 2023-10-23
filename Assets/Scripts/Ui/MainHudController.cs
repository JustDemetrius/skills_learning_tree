using System.Collections.Generic;
using System.Linq;
using Systems;
using Ui.HudButtons;
using Ui.Windows;
using UnityEngine;

namespace Ui
{
    public class MainHudController : IService
    {
        private GameObject _uiRootObject;
    
        private List<BaseWindow> _windows;
        private List<MainHudButton> _hudButtons;

        public MainHudController(GameObject uiRoot)
        {
            if (uiRoot == null)
            {
                Debug.LogError("UiRoot object is null");
                return;
            }
        
            _uiRootObject = uiRoot;
        }
        
        public virtual void Init()
        {
            CollectAndInitWindows();
            CollectAndInitHudButtons();
        }
    
        private void CollectAndInitWindows()
        {
            _windows = _uiRootObject.GetComponentsInChildren<BaseWindow>(true).ToList();
            _windows.ForEach(window => window.Init());
        }
    
        private void CollectAndInitHudButtons()
        {
            _hudButtons = _uiRootObject.GetComponentsInChildren<MainHudButton>(true).ToList();
            _hudButtons.ForEach(button =>
            {
                button.Init();
                button.OnButtonClicked += OnMainHudButtonClicked;
            });
        }

        private void OnMainHudButtonClicked(WindowType type)
        {
            var target = _windows.FirstOrDefault(w => w.WindowType == type);
            if (target == null) return;

            ChangeWindowActivity(!target.IsWindowOpened, target);
        }

        private void ChangeWindowActivity(bool isOpen, BaseWindow window)
        {
            if (window == null) return;
        
            if (isOpen) window.Show();
            else window.Hide();
        }
    }
}

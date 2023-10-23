using DG.Tweening;
using UnityEngine;

namespace Ui.HudButtons
{
    public class SkillsHudButton : MainHudButton
    {
        private Tween _clickTween;
        
        protected override void OnButtonClick()
        {
            _clickTween?.Kill(true);
            _clickTween = transform.DOPunchScale(Vector3.one * -0.1f, 0.35f)
                .SetEase(Ease.InOutBack);
            
            base.OnButtonClick();
        }

        protected override void OnValidate()
        {
            _targetWindowType = WindowType.SkillsTree;
            base.OnValidate();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Windows
{
    public class SkillsTreeItemView : MonoBehaviour
    {
        [SerializeField] private int _skillId;
        [SerializeField] private int _skillStepLevel;
        [SerializeField] private List<SkillsTreeItemView> _boundSkills;
        [Space] 
        [SerializeField] private Button _skillViewButton;
        [SerializeField] private TextMeshProUGUI _skillName;
        [Space]
        [Header("LearnIndication")]
        [SerializeField] private Image _learnIndicationImage;
        [SerializeField] private Color _learnedColor;
        [SerializeField] private Color _notLearnedColor;
        [Space]
        [Header("SelectIndications")]
        [SerializeField] private Image _selectionImage;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unSelectedColor;

        public event Action<SkillsTreeItemView> OnSkillViewClicked;
        
        public int ItemSkillId => _skillId;

        public SkillItemData SkillData { get; private set; }

        private Tween _clickTween;

        public SkillViewData GetViewData()
        {
            return new SkillViewData() 
            { 
                SkillViewID = _skillId,
                NeighboursIds = _boundSkills.Select(s => s.ItemSkillId).ToArray(), 
                SkillStepLevel = _skillStepLevel 
            };
        }
        
        public void InitItemView(SkillItemData data)
        {
            if (SkillData != null)
            {
                SkillData.OnLearnStateChanged -= UpdateLearnIndication;
            }
            
            SkillData = data;
            Redraw();
            ChangeSelectionState(false);

            _skillViewButton.onClick.AddListener(OnSkillClick);
            SkillData.OnLearnStateChanged += UpdateLearnIndication;
        }

        public void ChangeSelectionState(bool isSelected)
        {
            _selectionImage.color = isSelected ? _selectedColor : _unSelectedColor;
        }

        private void Redraw()
        {
            if (SkillData == null) return;

            _skillName.text = SkillData.Config.Name;
            UpdateLearnIndication();
        }
        
        private void UpdateLearnIndication()
        {
            _learnIndicationImage.color = SkillData.IsLearned ? _learnedColor : _notLearnedColor;
        }
        
        private void OnSkillClick()
        {
            _clickTween?.Kill(true);
            _clickTween = transform.DOPunchScale(Vector3.one * -0.1f, 0.35f)
                .SetEase(Ease.Linear);
            
            OnSkillViewClicked?.Invoke(this);
        }

        private void OnValidate()
        {
            _skillName.text = _skillId.ToString();
            gameObject.name = $"SkillWindowItemView [{_skillId}]";
        }
    }
}
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Windows
{
    public class SkillsTreeWindow : BaseWindow
    {
        #region SYSTEMS
        private SkillsLearningSystem _skillsLearningSystem;
        private CurrencySystem _currencySystem;
        #endregion

        [SerializeField] private Button _closeButton;
        [Space]
        [SerializeField] private List<SkillsTreeItemView> _skillsTreeItemViews;
        [SerializeField] private Button _addPointsButton;
        [SerializeField] private TextMeshProUGUI _currentPointsCountTMP;
        [SerializeField] private ScrollRect _skillsMapScroll;
        [Space] 
        [Header("SelectedSkills container")] 
        [SerializeField] private TextMeshProUGUI _skillIdTMP;
        [SerializeField] private TextMeshProUGUI _skillNameTMP;
        [SerializeField] private TextMeshProUGUI _skilDescriptionTMP;
        [SerializeField] private TextMeshProUGUI _skillCostTMP;
        [SerializeField] private Button _learnButton;
        [SerializeField] private Button _forgetButton;
        [SerializeField] private Button _forgetAllButton;
        
        private Tween _windowActivityTween;
        private SkillsTreeItemView _currentSelected;
        private Vector2 _defaultScrollMapPos;
        
        public override void Init()
        {
            _skillsLearningSystem = ServiceLocator.Get<SkillsLearningSystem>();
            _currencySystem = ServiceLocator.Get<CurrencySystem>();
            
            _skillsTreeItemViews.ForEach(view =>
            {
                var viewData = view.GetViewData();
                var data = _skillsLearningSystem.GetSkillData(viewData);
                view.InitItemView(data);
                view.OnSkillViewClicked += OnSkillViewClick;
            });
            
            _closeButton.onClick.AddListener(Hide);
            _learnButton.onClick.AddListener(OnLearnButtonClick);
            _forgetButton.onClick.AddListener(OnForgetButtonClick);
            _forgetAllButton.onClick.AddListener(_skillsLearningSystem.ResetAllLearnedSkills);
            _addPointsButton.onClick.AddListener(() => _skillsLearningSystem.AddLearnPoints());

            _defaultScrollMapPos = _skillsMapScroll.content.anchoredPosition;
            
            base.Init();
        }

        private void OnSkillViewClick(SkillsTreeItemView view)
        {
            if (view == null) return;
            
            if (_currentSelected) _currentSelected.ChangeSelectionState(false);
            _currentSelected = view;
            _currentSelected.ChangeSelectionState(true);
            RedrawSelectedSkillData();
        }

        private void RedrawCurrencyInfo()
        {
            var value = _currencySystem.GetCurrencyValueByType(CurrencyType.SkillLearnPoints).ToString(CultureInfo.InvariantCulture);
            _currentPointsCountTMP.text = $"Текущие очки: {value}";
        }
        
        private void RedrawSelectedSkillData()
        {
            if (_currentSelected == null)
            {
                _skillIdTMP.text = "Не выбрано";
                _skillNameTMP.text = "-";
                _skilDescriptionTMP.text = "-";
                _skillCostTMP.text = "-";
                _learnButton.interactable = false;
                _forgetButton.interactable = false;
            }
            else
            {
                var data = _currentSelected.SkillData;
                _skillIdTMP.text = $"ID: {data.ID}";
                _skillNameTMP.text = $"Имя: {data.Config.Name}";
                _skilDescriptionTMP.text = $"Описание: {data.Config.Description}";
                _skillCostTMP.text = $"Цена: {data.Config.PointsCost}";
                _learnButton.interactable = _skillsLearningSystem.IsAbleToLearnSkill(data);
                _forgetButton.interactable = _skillsLearningSystem.IsAbleToForgetSkill(data);
            }
        }
        
        private void OnCurrencyChanged(CurrencyType currencyType)
        {
            if (currencyType != CurrencyType.SkillLearnPoints) return;

            RedrawSelectedSkillData();
            RedrawCurrencyInfo();
        }
        
        private void OnLearnButtonClick()
        {
            if (_currentSelected == null ||
                _currentSelected.SkillData == null ||
                _skillsLearningSystem.IsAbleToLearnSkill(_currentSelected.SkillData) == false)
            {
                _learnButton.interactable = false;
                return;
            }
            
            _skillsLearningSystem.ChangeSkillLearnState(true, _currentSelected.SkillData.ID);
            RedrawSelectedSkillData();
        }
        
        private void OnForgetButtonClick()
        {
            if (_currentSelected == null ||
                _currentSelected.SkillData == null ||
                _skillsLearningSystem.IsAbleToForgetSkill(_currentSelected.SkillData) == false)
            {
                _forgetButton.interactable = false;
                return;
            }
            
            _skillsLearningSystem.ChangeSkillLearnState(false, _currentSelected.SkillData.ID);
            RedrawSelectedSkillData();
        }
        
        public override void Show()
        {
            if (IsWindowOpened) return;
            
            IsWindowOpened = true;

            _windowActivityTween?.Kill();
            gameObject.SetActive(true);
            _windowActivityTween = _windowContainer.transform.DOScale(Vector3.one, 0.35f)
                .SetEase(Ease.OutBack)
                .From(Vector3.zero);

            RedrawSelectedSkillData();
            RedrawCurrencyInfo();
            _currencySystem.OnCurrencyChanged += OnCurrencyChanged;
            _skillsMapScroll.content.anchoredPosition = _defaultScrollMapPos;
        }

        public override void Hide()
        {
            if (!IsWindowOpened) return;
            
            IsWindowOpened = false;

            _windowActivityTween?.Kill();

            _windowActivityTween = _windowContainer.transform.DOScale(Vector3.zero, 0.35f)
                .SetEase(Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false));
            
            if (_currentSelected) _currentSelected.ChangeSelectionState(false);
            _currentSelected = null;
            _currencySystem.OnCurrencyChanged -= OnCurrencyChanged;
        }

        protected override void OnValidate()
        {
            _windowType = WindowType.SkillsTree;
            base.OnValidate();
        }
    }
}
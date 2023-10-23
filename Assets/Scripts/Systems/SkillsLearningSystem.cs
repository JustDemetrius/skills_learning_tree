using System.Linq;
using Data;
using ScriptableObjects;

namespace Systems
{
    public class SkillsLearningSystem : IService
    {
        #region CONFIGS\SYSTEMS\FFACTORIES
        private SkillsConfig _skillsConfig;
        private SkillsDataFactory _skillsDataFactory;
        private CurrencySystem _currencySystem;
        #endregion

        public virtual void Init()
        {
            _skillsConfig = SkillsConfig.Instance;
            _skillsDataFactory = new SkillsDataFactory(_skillsConfig);
            _currencySystem = ServiceLocator.Get<CurrencySystem>();
        }

        public SkillItemData GetSkillData(SkillViewData viewData)
        {
            // if data with viewData.ID already exist this will return data without creating new one
            return _skillsDataFactory.CreateNewSkillData(viewData);
        }

        public void AddLearnPoints(int count = 1)
        {
            if (count <= 0) return;

            _currencySystem.TryChangeValueByType(CurrencyType.SkillLearnPoints, count);
        }

        public void ChangeSkillLearnState(bool isLearned, int id)
        {
            var data = _skillsDataFactory.GetSkillData(id);
            if (data == null ||
                isLearned && IsAbleToLearnSkill(data) == false ||
                !isLearned && IsAbleToForgetSkill(data) == false)
            {
                return;
            }

            if (data.IsLearned && isLearned == false)
            {
                ChangeLearnPointsCount(data.Config.PointsCost);
            }
            else
            {
                ChangeLearnPointsCount(data.Config.PointsCost * -1);
            }
            data.ChangeLearnState(isLearned);
        }

        public bool IsAbleToLearnSkill(SkillItemData skillData)
        {
            if (skillData == null) return false;
            
            var boundsSkillsData = _skillsDataFactory.GetAllBoundSkillData(skillData);
            if (boundsSkillsData == null) return false;

            float learnPoints = _currencySystem.GetCurrencyValueByType(CurrencyType.SkillLearnPoints);
            return skillData.IsLearned == false && 
                   learnPoints >= skillData.Config.PointsCost && 
                   boundsSkillsData.Exists(d => d.IsLearned);
        }

        public bool IsAbleToForgetSkill(SkillItemData skillData)
        {
            if (skillData == null || skillData.Config.IsBaseSkill || skillData.IsLearned == false) return false;
            
            var boundsSkillsData = _skillsDataFactory.GetAllBoundSkillData(skillData);
            if (boundsSkillsData == null) return false;

            var learnedNeighbours = boundsSkillsData.FindAll(d => d.IsLearned && !d.Config.IsBaseSkill);

            foreach (var neighbour in learnedNeighbours)
            {
                var neighbours = _skillsDataFactory.GetAllBoundSkillData(neighbour);
                if (neighbours.Exists(d => d.Config.IsBaseSkill)) continue;

                var learnedN = neighbours.FindAll(d => d.IsLearned && d.ID != skillData.ID);
                bool isExistLearnedParent = learnedN.Any(d =>
                    d.SkillStepLevel < neighbour.SkillStepLevel ||
                    d.SkillStepLevel == neighbour.SkillStepLevel &&
                    IsExistOlderSkillParent(d, neighbour.SkillStepLevel));

                if (isExistLearnedParent == false) return false;
            }

            return true;
        }

        public void ResetAllLearnedSkills()
        {
            var learnedSkills = _skillsDataFactory.GetAllLearnedSkills();
            int pointsToGet = 0;
            learnedSkills.ForEach(skillData =>
            {
                skillData.ChangeLearnState(false);
                pointsToGet += skillData.Config.PointsCost;
            });

            ChangeLearnPointsCount(pointsToGet);
        }

        private bool IsExistOlderSkillParent(SkillItemData target, int stepLevel)
        {
            return _skillsDataFactory.GetAllBoundSkillData(target)
                .Any(s => s.IsLearned && s.SkillStepLevel < stepLevel);
        }
        
        private void ChangeLearnPointsCount(int count)
        {
            _currencySystem.TryChangeValueByType(CurrencyType.SkillLearnPoints, count);
        }
    }
}
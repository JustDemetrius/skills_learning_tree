using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class SkillItemData
    {
        public event Action OnLearnStateChanged;
        
        public readonly int ID;
        public readonly int SkillStepLevel;
        public bool IsLearned { get; private set; } = false;
        public readonly SkillItemConfig Config;
        public readonly List<int> BoundSpellIds;

        public SkillItemData(SkillItemConfig config, SkillViewData viewData)
        {
            if (config == null) return;

            Config = config;
            ID = Config.ID;

            if (Config.IsBaseSkill) IsLearned = true;

            BoundSpellIds = viewData.NeighboursIds.ToList();
            SkillStepLevel = viewData.SkillStepLevel;
        }

        public void ChangeLearnState(bool isLearned)
        {
            if (IsLearned == isLearned || Config.IsBaseSkill) return;
            
            IsLearned = isLearned;
            OnLearnStateChanged?.Invoke();
        }
    }
}
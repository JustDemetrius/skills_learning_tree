using System.Collections.Generic;
using System.Linq;
using Data;
using ScriptableObjects;

namespace Systems
{
    public class SkillsDataFactory
    {
        private SkillsConfig _skillsConfig;
        
        private readonly Dictionary<int, SkillItemData> _skillsData = new();
        
        public SkillsDataFactory(SkillsConfig skillsConfig)
        {
            _skillsConfig = skillsConfig;
        }

        public SkillItemData GetSkillData(int skillId)
        {
            return _skillsData.ContainsKey(skillId) ? _skillsData[skillId] : null;
        }

        public SkillItemData CreateNewSkillData(SkillViewData viewData)
        {
            if (_skillsData.ContainsKey(viewData.SkillViewID))
            {
                return _skillsData[viewData.SkillViewID];
            }
            
            var config = _skillsConfig.GetConfigById(viewData.SkillViewID);
            if (config == null) return null;

            var data = new SkillItemData(config, viewData);
            _skillsData.Add(viewData.SkillViewID, data);
            return data;
        }

        public List<SkillItemData> GetAllLearnedSkills()
        {
            return _skillsData.Values.ToList().FindAll(data => data.IsLearned && data.Config.IsBaseSkill == false);
        }
        
        public List<SkillItemData> GetAllBoundSkillData(SkillItemData skillItemData)
        {
            if (skillItemData == null) return null;
            
            return _skillsData.Values.ToList().FindAll(data => skillItemData.BoundSpellIds.Contains(data.ID));
        }

        public List<int> GetAllBaseSkillsIds()
        {
            return _skillsData.Values.ToList().FindAll(data => data.Config.IsBaseSkill).Select(i => i.ID).ToList();
        }
    }
}
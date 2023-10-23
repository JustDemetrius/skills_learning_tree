using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(SkillsConfig), menuName = "Configs/SkillsConfig", order = 1)]
    public class SkillsConfig : ScriptableObject
    {
        private static SkillsConfig _instance;
        public static SkillsConfig Instance
        {
            get
            {
                if (_instance == null) _instance = Resources.Load<SkillsConfig>(nameof(SkillsConfig));
                return _instance;
            }
        }

        [SerializeField] private List<SkillItemConfig> _skillItemConfigs;

        public List<SkillItemConfig> GetAllSkillConfigs()
        {
            return _skillItemConfigs;
        }

        public SkillItemConfig GetConfigById(int id)
        {
            return _skillItemConfigs.FirstOrDefault(c => c.ID == id);
        }
    }
}
using System;

namespace Data
{
    [Serializable]
    public class SkillItemConfig
    {
        public int ID;
        public int PointsCost = 1;
        public string Name;
        public string Description;
        public bool IsBaseSkill = false;
    }
}
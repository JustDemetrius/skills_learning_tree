using System;
using System.Collections.Generic;
using Data;

namespace Systems
{
    public class CurrencySystem : IService
    {
        public event Action<CurrencyType> OnCurrencyChanged;

        private readonly Dictionary<CurrencyType, BaseCurrencyData> _currencyData = new();

        public virtual void Init()
        {
            _currencyData.Add(CurrencyType.SkillLearnPoints, new BaseCurrencyData(CurrencyType.SkillLearnPoints));
        }

        public float GetCurrencyValueByType(CurrencyType type)
        {
            return _currencyData.ContainsKey(type) ? _currencyData[type].CurrentValue : 0f;
        }

        public float TryChangeValueByType(CurrencyType type, float value)
        {
            if (!_currencyData.ContainsKey(type)) return 0f;

            var data = _currencyData[type];
            data.Change(value);
            
            OnCurrencyChanged?.Invoke(type);
            return data.CurrentValue;
        }
    }
}
using Systems;

namespace Data
{
    public class BaseCurrencyData
    {
        public readonly CurrencyType CurrencyType;
        public float CurrentValue { get; private set; }

        public BaseCurrencyData(CurrencyType type, float startValue = 0f)
        {
            CurrencyType = type;
            CurrentValue = startValue;
        }

        public float Change(float value)
        {
            CurrentValue = CurrentValue + value < 0f ? 0f : CurrentValue + value;
            
            return CurrentValue;
        }
    }
}
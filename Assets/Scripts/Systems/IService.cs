namespace Systems
{
    public interface IService
    {
        public virtual void Init()
        {
        }

        public virtual void Start()
        {
        }
        
        public virtual void Tick(float deltaTime)
        {
        }
    }
}
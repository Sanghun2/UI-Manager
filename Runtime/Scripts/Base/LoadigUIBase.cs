namespace BilliotGames
{
    public abstract class LoadigUIBase : UIBase
    {
        public abstract void StartLoading();
        public abstract void UpdateLoading(float currentValue, float totalValue);
        public abstract void StopLoading();
    }
}
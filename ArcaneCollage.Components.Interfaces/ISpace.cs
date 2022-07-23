namespace ArcaneCollage.Components.Interfaces
{
    public interface ISpace : IComponent
    {
        void OnEnter(object obj);
        void OnExist(object obj);
    }
}

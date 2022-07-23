namespace ArcaneCollage.Entities.Interfaces
{
    public interface ITime
    {
        int year { get; }
        int month { get; }
        int day { get; }
        int hour { get; }

        void Lapse();
    }
}

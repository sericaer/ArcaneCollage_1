using System.Collections.Generic;

namespace ArcaneCollage.Components.Interfaces
{
    public interface ISchedule : IComponent
    {
        public class Time
        {
            public int year;
            public int month;
            public int day;
            public int hour;
        }

        IEnumerable<(Time time, IPlan plan)> datas { get; }
    }
}

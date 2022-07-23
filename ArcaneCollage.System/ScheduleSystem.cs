using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcaneCollage.System
{
    public class ScheduleSystem
    {
        public void OnTimeLapse(IEnumerable<IEntity> entities, ITime time)
        {
            foreach(var entity in entities)
            {
                var schedule = entity.components.OfType<ISchedule>().SingleOrDefault();
                if(schedule == null)
                {
                    continue;
                }

                var plan = GeneratePlan(schedule, time);
                if(plan == null)
                {
                    continue;
                }

                entity.AddComponent(plan);
            }
        }

        private IPlan GeneratePlan(ISchedule schedule, ITime time)
        {
            foreach(var data in schedule.datas)
            {
                if(data.time.Equals(time))
                {
                    return data.plan;
                }
            }

            return null;
        }
    }
}

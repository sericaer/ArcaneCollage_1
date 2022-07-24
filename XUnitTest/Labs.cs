using ArcaneCollage.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUnitTest.Labs
{
    interface IBuliding
    {
        IUsage usage { get; }
        IGroup userGroup { get; set; }
    }

    public interface IUsage
    {
    }

    public interface IClassRoom : IUsage
    {

    }

    interface IPerson
    {
        IRole role { get; set; }
        ISchedule schedule { get; }
        IList<IPlan> plans { get; }

        IGroup userGroup { get; set; }
    }

    interface ISchedule
    {
        IEnumerable<(int hour, IPlan plan)> daily { get; }
        IEnumerable<(int day, int hour, IPlan plan)> monthly { get; }

        IEnumerable<(int month, int day, int hour, IPlan plan)> yearly { get; }

        IEnumerable<(int year, int month, int day, int hour, IPlan plan)> lifely { get; }
    }

    interface IRole
    {
    }

    interface Teacher : IRole
    {
        IPerson owner { get; }
    }

    interface Student : IRole
    {
        IPerson owner { get; }
    }

    interface IGroup
    {
        bool isFull { get; }
        IEnumerable<IPerson> persons { get; }

        void AddPerson(IPerson person);
        void Do();
    }

    interface ITeachLearnGroup : IGroup
    {
        IPerson teacher { get; }
        IEnumerable<IPerson> students { get; }
    }

    interface IPlan
    {

    }

    interface IPlanTeach : IPlan
    {

    }

    interface IPlanLearn : IPlan
    {

    }

    class TeachLearnGroup : ITeachLearnGroup
    {
        public IPerson teacher => throw new NotImplementedException();

        public IEnumerable<IPerson> students => throw new NotImplementedException();

        public IEnumerable<IPerson> persons => throw new NotImplementedException();

        public bool isFull => throw new NotImplementedException();

        public void AddPerson(IPerson person)
        {
            throw new NotImplementedException();
        }

        public void Do()
        {
            throw new NotImplementedException();
        }
    }

    class Person : IPerson
    {
        public IRole role { get; set; }

        public ISchedule schedule { get; }

        public IList<IPlan> plans { get; }
        public IGroup userGroup { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    class ScheduleSystem
    {
        void OnTimeLapse(IEnumerable<IPerson> persons, ITime time)
        {
            foreach (var person in persons)
            {
                var plan = person.schedule.GeneratePlan(time);
                if (plan == null)
                {
                    continue;
                }

                person.plans.Add(plan);
            }
        }
    }

    static class ScheduleSystem_Extentions
    {
        public static IPlan GeneratePlan(this ISchedule schedule, ITime time)
        {
            var element = schedule.daily.SingleOrDefault(x => x.hour == time.hour);
            return element.plan;
        }
    }

    class DoPlanTeach
    {
        void OnTimeLapse(IEnumerable<IPerson> persons, IEnumerable<IBuliding> bulidings, ITime time)
        {
            foreach (var person in persons)
            {
                var planTeach = person.plans.OfType<IPlanTeach>().SingleOrDefault();
                if(planTeach == null)
                {
                    continue;
                }

                var buliding = bulidings.Where(x => x.userGroup == null && x.usage is IClassRoom).FirstOrDefault();
                if(buliding == null)
                {
                    return;
                }

                var group = new TeachLearnGroup();
                buliding.userGroup = group;

                person.plans.Remove(planTeach);
            }
        }
    }

    class DoPlanLearn
    {
        void OnTimeLapse(IEnumerable<IPerson> persons, IEnumerable<IGroup> groups, ITime time)
        {
            foreach (var person in persons)
            {
                var plan = person.plans.OfType<IPlanLearn>().SingleOrDefault();
                if (plan == null)
                {
                    continue;
                }

                var group = groups.OfType<ITeachLearnGroup>().FirstOrDefault(x=>x.isFull);
                if(groups == null)
                {
                    return;
                }

                group.AddPerson(person);
                person.plans.Remove(plan);
            }
        }
    }

    class DoPlanDining
    {
        void OnTimeLapse(IEnumerable<IPerson> persons, IEnumerable<IGroup> groups, ITime time)
        {
            foreach (var person in persons)
            {
                var plan = person.plans.OfType<IPlanDining>().SingleOrDefault();
                if (plan == null)
                {
                    continue;
                }

                var group = groups.OfType<IDiningGroup>().FirstOrDefault(x => x.isFull);
                if (groups == null)
                {
                    return;
                }

                group.AddPerson(person);
                person.plans.Remove(plan);
            }
        }
    }

    internal interface IDiningGroup : IGroup
    {
    }

    internal interface IPlanDining : IPlan
    {
    }
}

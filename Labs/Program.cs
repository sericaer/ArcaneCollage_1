using ArcaneCollage.Entities;
using ArcaneCollage.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Labs
{
    interface IBuliding
    {
        string name { get; }
        IBulidingUsage usage { get; }
        IGroup userGroup { get; set; }
    }

    public interface IBulidingUsage
    {
    }

    public interface IClassRoomUsage : IBulidingUsage
    {

    }

    interface IPerson
    {
        string name { get; }
        IRole role { get; }
        ISchedule schedule { get; }
        IList<IPlan> plans { get; }

        IGroup group { get; set; }
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

    interface ITeacher : IRole
    {
        //IPerson owner { get; }
    }

    interface IStudent : IRole
    {
        //IPerson owner { get; }
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
        public IPerson teacher => _person.Where(x => x.role is ITeacher).SingleOrDefault();

        public IEnumerable<IPerson> students => _person.Where(x => x.role is IStudent);

        public IEnumerable<IPerson> persons => _person;

        public bool isFull =>  false;

        private IList<IPerson> _person = new List<IPerson>();

        public void AddPerson(IPerson person)
        {
            _person.Add(person);
            person.group = this;
        }

        public void Do()
        {
            throw new NotImplementedException();
        }
    }

    class Person : IPerson
    {
        public static Func<IRole, ISchedule> GetRoleSchedule;

        public string name { get; }

        public IRole role { get; }

        public ISchedule schedule { get; }

        public IList<IPlan> plans { get; } = new List<IPlan>();
        public IGroup group { get ; set ; }


        public Person(string name, IRole role)
        {
            this.name = name;
            this.role = role;
            this.schedule = GetRoleSchedule(role);
        }
    }

    class ScheduleSystem
    {
        public void OnTimeLapse(IEnumerable<IPerson> persons, ITime time)
        {
            foreach (var person in persons)
            {
                var plan = person.schedule.GeneratePlan(time);
                if (plan == null)
                {
                    continue;
                }

                Console.WriteLine($"{person.name}[{person.role.GetType().Name}] Want {plan.GetType().Name}");

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

    class DoPlanTeachSystem
    {
        public void OnTimeLapse(IEnumerable<IPerson> persons, IEnumerable<IBuliding> bulidings, ITime time)
        {
            foreach (var person in persons)
            {
                var plan = person.plans.OfType<IPlanTeach>().SingleOrDefault();
                if (plan == null)
                {
                    continue;
                }

                var buliding = bulidings.Where(x => x.userGroup == null && x.usage is IClassRoomUsage).FirstOrDefault();
                if (buliding == null)
                {
                    return;
                }

                var group = new TeachLearnGroup();
                group.AddPerson(person);

                buliding.userGroup = group;

                person.plans.Remove(plan);

                Console.WriteLine($"{person.name}[{person.role.GetType().Name}] finish plan {plan.GetType().Name} : Occpy {buliding.name} to StartTeach");
            }
        }
    }

    class DoPlanLearnSystem
    {
        public void OnTimeLapse(IEnumerable<IPerson> persons, IEnumerable<IGroup> groups, ITime time)
        {
            foreach (var person in persons)
            {
                var plan = person.plans.OfType<IPlanLearn>().SingleOrDefault();
                if (plan == null)
                {
                    continue;
                }

                var group = groups.OfType<ITeachLearnGroup>().FirstOrDefault(x => !x.isFull);
                if (groups == null)
                {
                    return;
                }

                group.AddPerson(person);
                person.plans.Remove(plan);

                Console.WriteLine($"{person.name}[{person.role.GetType().Name}] finish plan {plan.GetType().Name} : Join in group {group.teacher.name} to StartLean");
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

                var group = groups.OfType<IDiningGroup>().FirstOrDefault(x => !x.isFull);
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

    internal class Teacher : ITeacher
    {
        public Teacher()
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var schedules = new Dictionary<IRole, ISchedule>();

            Person.GetRoleSchedule = (role) =>
            {
                return schedules[role];
            };

            var teacherRole = new Teacher();
            var studentRole = new Student();
            schedules.Add(teacherRole, new Schedule()
            {
                daily = new (int hour, IPlan plan)[] { (6, new PlanDining()), (7, new PlanTeach()) , (13, new PlanSleep()) }
            });

            schedules.Add(studentRole, new Schedule()
            {
                daily = new (int hour, IPlan plan)[] { (7, new PlanDining()), (8, new PlanLearn()), (12, new PlanSleep()) }
            });

            var persons = new IPerson[]
            {
                new Person("P0", teacherRole),
                new Person("P1", studentRole),
            };

            var classroom = new ClassroomUsage();
            var buildings = new IBuliding[]
            {
                new Building("B0", classroom),
                new Building("B1", classroom),
                new Building("B2", classroom),
            };

            var time = new Time(1, 1, 1, 7);

            var scheduleSytem = new ScheduleSystem();
            var doPlanTeachSystem = new DoPlanTeachSystem();
            var doPlanLearnSystem = new DoPlanLearnSystem();

            for (int i=0; i<100; i++)
            {
                Console.WriteLine($"{time.year}-{time.month}-{time.day} {time.hour}");

                scheduleSytem.OnTimeLapse(persons, time);
                doPlanTeachSystem.OnTimeLapse(persons, buildings, time);
                doPlanLearnSystem.OnTimeLapse(persons, buildings.Select(x => x.userGroup), time);

                time.Lapse();


                Thread.Sleep(1000);
            }
        }
    }

    internal class ClassroomUsage : IClassRoomUsage
    {

    }

    internal class Building : IBuliding
    {
        public Building(string name, IBulidingUsage usage)
        {
            this.name = name;
            this.usage = usage;
        }

        public IBulidingUsage usage { get; }

        public IGroup userGroup { get; set; }

        public string name { get; }
    }

    internal class Student : IStudent
    {
        public Student()
        {
        }
    }

    internal class PlanLearn : IPlanLearn
    {
    }

    internal class PlanTeach : IPlanTeach
    {
    }

    internal class PlanDining : IPlanDining
    {
    }

    internal class PlanSleep : IPlan
    {

    }

    internal class Schedule : ISchedule
    {
        public IEnumerable<(int hour, IPlan plan)> daily { get; set; }

        public IEnumerable<(int day, int hour, IPlan plan)> monthly => throw new NotImplementedException();

        public IEnumerable<(int month, int day, int hour, IPlan plan)> yearly => throw new NotImplementedException();

        public IEnumerable<(int year, int month, int day, int hour, IPlan plan)> lifely => throw new NotImplementedException();
    }
}

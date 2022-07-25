using ArcaneCollage.Entities;
using ArcaneCollage.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Labs.World1
{
    public interface IBuilding
    {
        string name { get; }
        USAGE usage { get; }
        IGroup userGroup { get; }
        enum USAGE
        {
            TEACH_LEARN,
            DINNING,
            SLEEP
        }
    }

    public class Building : IBuilding
    {
        internal static Func<IBuilding, IGroup> GetGroup;

        internal static Action<IBuilding> RegistBuildingUsage;

        internal static Action<IGroup> RegistGroup;
        internal static Action<IGroup> UnRegistGroup;

        public string name { get; }
        public IBuilding.USAGE usage { get; }
        public IGroup userGroup => GetGroup(this);

        public Building(string name, IBuilding.USAGE usage)
        {
            this.name = name;
            this.usage = usage;

            RegistBuildingUsage(this);
        }
    }


    public interface IBuildingUsage
    {
        bool isFull { get; }
        IEnumerable<IPerson> persons { get; }
    }

    public interface IPerson
    {
        enum ROLE
        {
            TEACHER,
            STUDENT
        }

        string name { get; }
        ROLE role { get; }
        ISchedule schedule { get; }
        IEnumerable<IPlan> plans { get; }

        IGroup group { get; }


        public void AddPlan(IPlan plan);
        public void RemovePlan(IPlan plan);
    }

    class Person : IPerson
    {
        public static Func<IPerson.ROLE, ISchedule> GetRoleSchedule;
        public static Func<IPerson, IGroup> GetGroup;

        public static Action<IPerson> RegistPlanSystem;

        public static Action<IPerson, IPlan> RegistDoPlanSystem;
        public static Action<IPerson, IPlan> UnRegistDoPlanSystem;

        public string name { get; }

        public IPerson.ROLE role { get; }

        public ISchedule schedule { get; }

        public IEnumerable<IPlan> plans => _plans;

        public IGroup group => GetGroup(this);

        private IList<IPlan> _plans = new List<IPlan>();

        public Person(string name, IPerson.ROLE role)
        {
            this.name = name;
            this.role = role;
            this.schedule = GetRoleSchedule(role);

            RegistPlanSystem(this);
        }

        public void AddPlan(IPlan plan)
        {
            _plans.Add(plan);
            RegistDoPlanSystem(this, plan);
        }

        public void RemovePlan(IPlan plan)
        {
            _plans.Remove(plan);
            UnRegistDoPlanSystem(this, plan);
        }
    }

    public interface ISchedule
    {
        IEnumerable<(int hour, IPlan plan)> daily { get; }
        IEnumerable<(int day, int hour, IPlan plan)> monthly { get; }

        IEnumerable<(int month, int day, int hour, IPlan plan)> yearly { get; }

        IEnumerable<(int year, int month, int day, int hour, IPlan plan)> lifely { get; }
    }

    internal class Schedule : ISchedule
    {
        public IEnumerable<(int hour, IPlan plan)> daily { get; set; }

        public IEnumerable<(int day, int hour, IPlan plan)> monthly => throw new NotImplementedException();

        public IEnumerable<(int month, int day, int hour, IPlan plan)> yearly => throw new NotImplementedException();

        public IEnumerable<(int year, int month, int day, int hour, IPlan plan)> lifely => throw new NotImplementedException();
    }

    public interface ClassRoomUsage : IBuildingUsage
    {
        IPerson teacher { get; }
        IEnumerable<IPerson> students { get; }
    }

    public interface IPlan
    {

    }

    public interface IPlanTeach : IPlan
    {

    }

    internal interface IPlanLearn : IPlan
    {
    }

    internal class PlanLearn : IPlanLearn
    {
    }


    internal interface IPlanDining : IPlan
    {
    }

    internal interface IPlanSleep : IPlan
    {
    }

    internal class PlanTeach : IPlanTeach
    {
    }

    internal class PlanDining : IPlanDining
    {
    }

    internal class PlanSleep : IPlanSleep
    {

    }

    public interface IGroup
    {
        bool isFull { get; }
        IEnumerable<IPerson> persons { get; }
        IBuilding building { get; }
        bool vaild { get; }

        void AddPerson(IPerson person);
        void RemovePerson(IPerson person);
    }

    public interface ITeachLearnGroup : IGroup
    {
        IPerson teacher { get; }
        IEnumerable<IPerson> students { get; }
    }

    public abstract class Group : IGroup
    {
        public static Action<IGroup> RegistGroup;
        public static Action<IGroup> UnRegistGroup;

        public abstract bool isFull { get; }
        public IEnumerable<IPerson> persons => _persons;
        public IBuilding building { get; }

        public abstract bool vaild { get; }

        private List<IPerson> _persons = new List<IPerson>();

        public Group(IBuilding building)
        {
            this.building = building;

            RegistGroup(this);
        }

        public void AddPerson(IPerson person)
        {
            _persons.Add(person);
        }

        public void RemovePerson(IPerson person)
        {
            _persons.Remove(person);
        }
    }

    public class TeachLearnGroup : Group, ITeachLearnGroup
    {
        public TeachLearnGroup(IBuilding building, IPerson teacher) : base(building)
        {
            AddPerson(teacher);
        }

        public IPerson teacher => persons.SingleOrDefault(x => x.role == IPerson.ROLE.TEACHER);

        public IEnumerable<IPerson> students => persons.Where(x => x.role == IPerson.ROLE.STUDENT);

        public override bool vaild => teacher != null;

        public override bool isFull => students.Count() >= 2;
    }

    public class TeachLearnSystem
    {
        List<ClassRoomUsage> teachLearns = new List<ClassRoomUsage>();

        public void OnTimeLapse()
        {
            foreach(var element in teachLearns)
            {

            }
        }
    }

    public class DoPlanTeachSystem
    {
        public List<IPerson> persons = new List<IPerson>();
        public List<IBuilding> teachLearnBuildings = new List<IBuilding>();

        private List<IPerson> needRemovePlan = new List<IPerson>();

        public void OnTimeLapse(ITime time)
        {
            foreach (var person in persons)
            {
                var plan = person.plans.OfType<IPlanTeach>().SingleOrDefault();
                if(plan == null)
                {
                    throw new Exception();
                }

                var building = teachLearnBuildings.Where(x=>x.userGroup == null).FirstOrDefault();
                if (building == null)
                {
                    Console.WriteLine($"{person.name}[{person.role.GetType().Name}] NOT FINISH {plan.GetType().Name} : no idle building");

                    break;
                }

                var group = new TeachLearnGroup(building, person);
                needRemovePlan.Add(person);

                Console.WriteLine($"{person.name}[{person.role.GetType().Name}] FINISH {plan.GetType().Name} : Occpy {building.name} to teach");
            }

            foreach(var person in needRemovePlan)
            {
                person.RemovePlan(person.plans.OfType<IPlanTeach>().SingleOrDefault());
            }

            needRemovePlan.Clear();
        }
    }

    public class DoPlanLearnSystem
    {
        public List<ITeachLearnGroup> groups = new List<ITeachLearnGroup>();
        public List<IPerson> persons = new List<IPerson>();

        private List<IPerson> needRemovePlan = new List<IPerson>();

        public void OnTimeLapse(ITime time)
        {
            foreach (var person in persons)
            {
                var plan = person.plans.OfType<IPlanLearn>().SingleOrDefault();
                if (plan == null)
                {
                    throw new Exception();
                }

                var group = groups.Where(x => !x.isFull).FirstOrDefault();
                if (group == null)
                {
                    Console.WriteLine($"{person.name}[{person.role.GetType().Name}] NOT FINISH {plan.GetType().Name} : no vaild group");

                    break;
                }

                group.AddPerson(person);
                needRemovePlan.Add(person);

                Console.WriteLine($"{person.name}[{person.role.GetType().Name}] FINISH {plan.GetType().Name} : Join in Group {group.teacher.name} to learn");
            }

            foreach (var person in needRemovePlan)
            {
                person.RemovePlan(person.plans.OfType<IPlanLearn>().SingleOrDefault());
            }

            needRemovePlan.Clear();
        }
    }

    class ScheduleSystem
    {
        public List<IPerson> persons = new List<IPerson>();

        public void OnTimeLapse(ITime time)
        {
            foreach (var person in persons)
            {
                var plan = GeneratePlan(person.schedule, time);
                if (plan == null)
                {
                    continue;
                }

                Console.WriteLine($"{person.name}[{person.role.GetType().Name}] {plan.GetType().Name}");

                person.AddPlan(plan);
            }
        }

        private static IPlan GeneratePlan(ISchedule schedule, ITime time)
        {
            var element = schedule.daily.SingleOrDefault(x => x.hour == time.hour);
            return element.plan;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var schedules = new Dictionary<IPerson.ROLE, ISchedule>()
            {
                {
                    IPerson.ROLE.TEACHER,  
                    new Schedule()
                    {
                        daily = new (int hour, IPlan plan)[] { (6, new PlanDining()), (7, new PlanTeach()) , (13, new PlanSleep()) }
                    }
                },
                {
                    IPerson.ROLE.STUDENT,
                    new Schedule()
                    {
                        daily = new (int hour, IPlan plan)[] { (7, new PlanDining()), (8, new PlanLearn()), (12, new PlanSleep()) }
                    }
                }
            };

            var persons = new List<IPerson>();
            var buildings = new List<IBuilding>();

            var scheduleSystem = new ScheduleSystem();
            var doPlanTeachSystem = new DoPlanTeachSystem();
            var doGroupSystem = new DoGroupSystem();
            var doPlanLearnSystem = new DoPlanLearnSystem();

            Person.GetRoleSchedule = (role) =>
            {
                return schedules[role];
            };

            Person.GetGroup = (person) =>
            {
                return doGroupSystem.groups.SingleOrDefault(x => x.persons.Contains(person));
            };

            Person.RegistPlanSystem = (person) =>
            {
                scheduleSystem.persons.Add(person);
            };

            Person.RegistDoPlanSystem = (person, plan) =>
            {
                var group = person.group;
                if (group != null)
                {
                    group.RemovePerson(person);
                    if(!group.vaild)
                    {
                        Group.UnRegistGroup(group);
                    }

                    //Console.WriteLine($"{person.name}[{person.role.GetType().Name}] Release {person.group.building.name}");

                    //doGroupSystem.groups.Remove(person.group);
                }

                switch (plan)
                {
                    case IPlanTeach teach:
                        doPlanTeachSystem.persons.Add(person);
                        break;
                    case IPlanLearn learn:
                        doPlanLearnSystem.persons.Add(person);
                        break;
                }
            };

            Person.UnRegistDoPlanSystem = (person, plan) =>
            {
                switch (plan)
                {
                    case IPlanTeach teach:
                        doPlanTeachSystem.persons.Remove(person);
                        break;
                    case IPlanLearn learn:
                        doPlanLearnSystem.persons.Remove(person);
                        break;
                }
            };


            Building.RegistBuildingUsage = (building) =>
            {
                switch(building.usage)
                {
                    case IBuilding.USAGE.TEACH_LEARN:
                        doPlanTeachSystem.teachLearnBuildings.Add(building);
                        break;
                }
            };

            Building.GetGroup = (building) =>
            {
                 return doGroupSystem.groups.SingleOrDefault(x => x.building == building);
            };

            Group.RegistGroup = (group) =>
            {
                doGroupSystem.groups.Add(group);

                switch (group)
                {
                    case ITeachLearnGroup teachLearn:
                        doPlanLearnSystem.groups.Add(teachLearn);
                        break;
                }
            };

            Group.UnRegistGroup = (group) =>
            {
                doGroupSystem.groups.Remove(group);

                switch (group)
                {
                    case ITeachLearnGroup teachLearn:
                        doPlanLearnSystem.groups.Remove(teachLearn);
                        break;
                }
            };

            var time = new Time(1, 1, 1, 6);

            persons.Add(new Person("PT0", IPerson.ROLE.TEACHER));
            persons.Add(new Person("PT1", IPerson.ROLE.TEACHER));
            persons.Add(new Person("PS0", IPerson.ROLE.STUDENT));
            persons.Add(new Person("PS1", IPerson.ROLE.STUDENT));
            persons.Add(new Person("PS2", IPerson.ROLE.STUDENT));
            persons.Add(new Person("PS3", IPerson.ROLE.STUDENT));
            persons.Add(new Person("PS4", IPerson.ROLE.STUDENT));


            buildings.Add(new Building("B0", IBuilding.USAGE.TEACH_LEARN));
            buildings.Add(new Building("B1", IBuilding.USAGE.TEACH_LEARN));

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"{time.year}-{time.month}-{time.day} {time.hour}");

                scheduleSystem.OnTimeLapse(time);
                doPlanTeachSystem.OnTimeLapse(time);
                doPlanLearnSystem.OnTimeLapse(time);

                //doTeachLearnSystem.OnTimeLapse(groups, time);

                time.Lapse();

                Thread.Sleep(1000);
            }

            Console.WriteLine("This is World1");
        }
    }

    internal class DoGroupSystem
    {

        public List<IGroup> groups = new List<IGroup>();

        public DoGroupSystem()
        {
        }
    }
}

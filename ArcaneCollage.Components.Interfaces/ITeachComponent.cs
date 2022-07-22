using ArcaneCollage.Skills.Interfaces;

namespace ArcaneCollage.Components.Interfaces
{
    public interface ITeachComponent : IComponent
    {
        SkillType skillType { get; }
        double value { get; }


        void AddDetail(string name, double value);
    }
}

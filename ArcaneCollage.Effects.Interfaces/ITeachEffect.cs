using ArcaneCollage.Skills.Interfaces;

namespace ArcaneCollage.Effects.Interfaces
{
    public interface ITeachEffect : IEffect
    {
        double GetEffectValue(SkillType skillType);
    }
}

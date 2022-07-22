using ArcaneCollage.Skills.Interfaces;

namespace ArcaneCollage.Effects.Interfaces
{
    public interface ILearnEffect : IEffect
    {
        double GetEffectValue(SkillType skillType);
    }
}

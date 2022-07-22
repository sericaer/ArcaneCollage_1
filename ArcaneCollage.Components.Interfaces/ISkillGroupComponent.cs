using ArcaneCollage.Skills.Interfaces;
using System.Collections.Generic;

namespace ArcaneCollage.Components.Interfaces
{
    public interface ISkillGroupComponent : IComponent
    {
        Dictionary<SkillType, ISkill> dict { get; }
    }
}

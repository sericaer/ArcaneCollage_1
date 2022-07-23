using ArcaneCollage.Effects.Interfaces;
using System.Collections.Generic;

namespace ArcaneCollage.Components.Interfaces
{
    public interface ITeacherLabel : IComponent
    {
        IEnumerable<ITeachEffect> teachEffects { get; }
    }
}

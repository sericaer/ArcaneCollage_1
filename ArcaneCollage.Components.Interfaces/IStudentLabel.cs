using ArcaneCollage.Effects.Interfaces;
using System.Collections.Generic;

namespace ArcaneCollage.Components.Interfaces
{
    public interface IStudentLabel : IComponent
    {
        IEnumerable<ILearnEffect> learnEffects { get; }

        void AddLearn(ILearnComponent learn);
    }
}

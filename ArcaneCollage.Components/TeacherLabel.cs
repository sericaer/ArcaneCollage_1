using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Effects.Interfaces;
using ArcaneCollage.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcaneCollage.Components
{
    public class TeacherLabel : ITeacherLabel
    {
        public IEnumerable<ITeachEffect> teachEffects => owner.components.OfType<ITeachEffect>();

        private IEntity owner { get; }

        public TeacherLabel(IEntity owner)
        {
            this.owner = owner;
        }
    }

    public class StudentLabel : IStudentLabel
    {
        public IEnumerable<ILearnEffect> learnEffects => owner.components.OfType<ILearnEffect>();

        private IEntity owner { get; }

        public StudentLabel(IEntity owner)
        {
            this.owner = owner;
        }

        public void AddLearn(ILearnComponent learn)
        {
            owner.AddComponent(learn);
        }
    }
}

using ArcaneCollage.Components;
using ArcaneCollage.Effects.Interfaces;
using ArcaneCollage.Entities.Interfaces;
using ArcaneCollage.Skills.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcaneCollage.System
{
    public class TeachSystem
    {
        public void OnTimeLapse(IEnumerable<ITeachLearnSpace> spaces)
        {
            foreach(var space in spaces)
            {
                var teach = GenerateTeach(space.teacher, space.skillType);
                space.teacher.AddComponent(teach);

                foreach (var student in space.students)
                {
                    var lean = GenerateLearn(student, space.skillType);

                    lean.skillType = teach.skillType;
                    lean.AddDetail("Teach", teach.value);

                    student.AddComponent(lean);
                }

                space.teacher.RemoveComponent(teach);
            }
        }

        private LearnComponent GenerateLearn(IEntity student, SkillType skillType)
        {
            var learn = new LearnComponent();

            var effects = student.components.OfType<ILearnEffect>();
            foreach(var effect in effects)
            {
                var value = effect.GetEffectValue(skillType);
                learn.AddDetail(effect.name, value);
            }

            return learn;
        }

        private TeachComponent GenerateTeach(IEntity teacher, SkillType skillType)
        {
            var teach = new TeachComponent();
            teach.skillType = skillType;

            var effects = teacher.components.OfType<ITeachEffect>();
            foreach (var effect in effects)
            {
                var value = effect.GetEffectValue(skillType);
                teach.AddDetail(effect.name, value);
            }

            return teach;
        }
    }
}

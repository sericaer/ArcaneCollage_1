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
        void OnTimeLapse(ITeachLearnSpace space)
        {
            var tech = GenerateTeach(space.teacher, space.skillType);
            space.teacher.AddComponent(tech);

            foreach (var student in space.students)
            {
                var lean = GenerateLearn(student, space.skillType);

                lean.skillType = tech.skillType;
                lean.AddDetail("Teach", tech.value);

                student.AddComponent(lean);
            }

            space.teacher.RemoveComponent(tech);
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

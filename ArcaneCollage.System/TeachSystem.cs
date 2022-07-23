using ArcaneCollage.Components;
using ArcaneCollage.Components.Interfaces;
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
            foreach (var space in spaces)
            {
                var teach = GenerateTeach(space.teacher, space.skillType);

                foreach (var student in space.students)
                {
                    var learn = GenerateLearn(student, space.skillType);

                    learn.skillType = teach.skillType;
                    learn.AddDetail("Teach", teach.value);

                    student.AddLearn(learn);
                }
            }
        }

        private LearnComponent GenerateLearn(IStudentLabel student, SkillType skillType)
        {
            var learn = new LearnComponent();

            foreach (var effect in student.learnEffects)
            {
                var value = effect.GetEffectValue(skillType);
                learn.AddDetail(effect.name, value);
            }

            return learn;
        }

        private TeachComponent GenerateTeach(ITeacherLabel teacher, SkillType skillType)
        {
            var teach = new TeachComponent();
            teach.skillType = skillType;

            foreach (var effect in teacher.teachEffects)
            {
                var value = effect.GetEffectValue(skillType);
                teach.AddDetail(effect.name, value);
            }

            return teach;
        }

        //public void OnTimeLapse(IEnumerable<ITeachLearnSpace> spaces)
        //{
        //    foreach(var space in spaces)
        //    {
        //        var teach = GenerateTeach(space.teacher, space.skillType);
        //        space.teacher.AddTeach(teach);

        //        foreach (var student in space.students)
        //        {
        //            var learn = GenerateLearn(student, space.skillType);

        //            learn.skillType = teach.skillType;
        //            learn.AddDetail("Teach", teach.value);

        //            student.AddLearn(learn);
        //        }

        //        space.teacher.RemoveTeach(teach);
        //    }
        //}

        //private LearnComponent GenerateLearn(IStudentLabel student, SkillType skillType)
        //{
        //    var learn = new LearnComponent();

        //    var effects = student.components.OfType<ILearnEffect>();
        //    foreach(var effect in effects)
        //    {
        //        var value = effect.GetEffectValue(skillType);
        //        learn.AddDetail(effect.name, value);
        //    }

        //    return learn;
        //}

        //private TeachComponent GenerateTeach(ITeacherLabel teacher, SkillType skillType)
        //{
        //    var teach = new TeachComponent();
        //    teach.skillType = skillType;

        //    var effects = teacher.components.OfType<ITeachEffect>();
        //    foreach (var effect in effects)
        //    {
        //        var value = effect.GetEffectValue(skillType);
        //        teach.AddDetail(effect.name, value);
        //    }

        //    return teach;
        //}
    }
}

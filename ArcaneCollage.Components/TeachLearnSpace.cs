using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Entities.Interfaces;
using ArcaneCollage.Skills.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcaneCollage.Components
{
    public class TeachLearnSpace : ITeachLearnSpace
    {
        public SkillType skillType => throw new NotImplementedException();

        public ITeacherLabel teacher => entities.SelectMany(x => x.components.OfType<ITeacherLabel>()).SingleOrDefault();

        public IEnumerable<IStudentLabel> students => entities.SelectMany(x => x.components.OfType<IStudentLabel>());

        private List<IEntity> entities = new List<IEntity>();

        public bool isStudentFull()
        {
            return false;
        }

        public void OnEnter(object obj)
        {
            entities.Add(obj as IEntity);
        }

        public void OnExist(object obj)
        {
            entities.Remove(obj as IEntity);
        }
    }
}

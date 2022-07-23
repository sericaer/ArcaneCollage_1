using ArcaneCollage.Skills.Interfaces;
using System.Collections.Generic;

namespace ArcaneCollage.Components.Interfaces
{
    public interface ITeachLearnSpace : ISpace
    {
        SkillType skillType { get; }
        ITeacherLabel teacher { get; }
        IEnumerable<IStudentLabel> students { get; }

        bool isStudentFull();
    }
}

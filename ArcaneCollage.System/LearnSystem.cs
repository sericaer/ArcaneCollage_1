using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Entities.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ArcaneCollage.System
{
    public class LearnSystem
    {
        void OnTimeLapse(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                foreach(var learn in entity.components.OfType<ILearnComponent>().ToArray())
                {
                    var skillGroup = entity.components.OfType<ISkillGroupComponent>().SingleOrDefault();
                    skillGroup.dict[learn.skillType].knowledge += learn.value;

                    entity.RemoveComponent(learn);
                }
            }
        }
    }
}

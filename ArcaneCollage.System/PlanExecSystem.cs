using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcaneCollage.System
{
    class PlanExecSystem
    {
        private Dictionary<ActionType, Action<IEntity, IEnumerable<IEntity>>> planExecMap = new Dictionary<ActionType, Action<IEntity, IEnumerable<IEntity>>>()
        {
            { ActionType.Eat, GotoEat},
            { ActionType.Learn, GotoLearn},
            { ActionType.Learn, GotoTeach}
        };

        public void OnTimeLapse(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                var plan = entity.components.OfType<IPlan>().SingleOrDefault();
                if (plan == null)
                {
                    continue;
                }

                planExecMap[plan.actionType](entity, entities);

                entity.RemoveComponent(plan);
            }
        }

        private static void GotoEat(IEntity target, IEnumerable<IEntity> entities)
        {
            var location = target.components.OfType<ILocation>().SingleOrDefault();

            foreach (var entity in entities)
            {
                var space = entity.components.OfType<IEatPlace>().SingleOrDefault();
                if (space == null)
                {
                    continue;
                }

                if(space.isFull)
                {
                    continue;
                }

                location.space = space;
            }

        }

        private static void GotoLearn(IEntity target, IEnumerable<IEntity> entities)
        {
            var location = target.components.OfType<ILocation>().SingleOrDefault();

            foreach (var entity in entities)
            {
                var space = entity.components.OfType<ITeachLearnSpace>().SingleOrDefault();
                if (space == null)
                {
                    continue;
                }

                if (space.isStudentFull())
                {
                    continue;
                }

                location.space = space;
            }
        }

        private static void GotoTeach(IEntity target, IEnumerable<IEntity> entities)
        {
            var location = target.components.OfType<ILocation>().SingleOrDefault();

            foreach (var entity in entities)
            {
                var space = entity.components.OfType<ITeachLearnSpace>().SingleOrDefault();
                if (space == null)
                {
                    continue;
                }

                if (space.teacher != null)
                {
                    continue;
                }

                location.space = space;
            }
        }

    }
}

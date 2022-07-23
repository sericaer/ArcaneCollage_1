using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Entities;
using ArcaneCollage.Entities.Interfaces;
using ArcaneCollage.Sessions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcaneCollage.Sessions
{
    public partial class Session : ISession
    {
        public ITime time { get; private set; }

        public IEnumerable<IEntity> entities { get; private set; }

        public IEnumerable<IEntity> teachers => entities.Where(x => x.components.Any(y => y is ITeacherLabel));

        public IEnumerable<IEntity> students => entities.Where(x => x.components.Any(y => y is IStudentLabel));

        public IEnumerable<IEntity> buildings => entities.Where(x => x.components.Any(y => y is ISpace));

        public void OnTimeLapse()
        {
            time.Lapse();
        }
    }
}

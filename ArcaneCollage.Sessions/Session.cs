using ArcaneCollage.Entities.Interfaces;
using ArcaneCollage.Sessions.Interfaces;
using System;
using System.Collections.Generic;

namespace ArcaneCollage.Sessions
{
    public partial class Session : ISession
    {
        public ITime time { get; private set; }

        public IEnumerable<IEntity> entities => throw new NotImplementedException();

        public IEnumerable<IEntity> teachers => throw new NotImplementedException();

        public IEnumerable<IEntity> students => throw new NotImplementedException();

        public IEnumerable<IEntity> buildings => throw new NotImplementedException();

        public void OnTimeLapse()
        {
            time.Lapse();
        }
    }
}

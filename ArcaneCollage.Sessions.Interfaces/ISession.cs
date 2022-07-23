using ArcaneCollage.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace ArcaneCollage.Sessions.Interfaces
{
    public interface ISession
    {
        ITime time { get; }
        IEnumerable<IEntity> entities { get; }

        IEnumerable<IEntity> teachers { get; }

        IEnumerable<IEntity> students { get; }

        IEnumerable<IEntity> buildings { get; }

        void OnTimeLapse();

    }
}

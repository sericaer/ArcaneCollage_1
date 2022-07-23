using ArcaneCollage.Entities;
using System;

namespace ArcaneCollage.Sessions
{
    public partial class Session
    {
        public static class Builder
        {
            public static Session Build()
            {
                var session = new Session();
                session.time = new Time(1,1,1, 8);

                return session;
            }
        }
    }
}

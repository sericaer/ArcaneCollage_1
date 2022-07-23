using ArcaneCollage.Components.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcaneCollage.Components
{
    public class Plan : IPlan
    {
        public ActionType actionType { get; }

        public Plan(ActionType actionType)
        {
            this.actionType = actionType;
        }
    }
}

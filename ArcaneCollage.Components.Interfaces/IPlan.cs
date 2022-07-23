using System;
using System.Collections.Generic;
using System.Text;

namespace ArcaneCollage.Components.Interfaces
{
    public interface IPlan : IComponent
    {
        ActionType actionType { get; }
    }

    public enum ActionType
    {
        Eat,
        Learn,
        Teach,
        Sleep,
    }
}

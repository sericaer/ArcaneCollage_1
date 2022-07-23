using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Skills.Interfaces;
using System;
using System.Collections.Generic;

namespace ArcaneCollage.Entities.Interfaces
{
    public interface IEntity
    {
        IEnumerable<IComponent> components { get; }
        void AddComponent(IComponent component);
        void RemoveComponent(IComponent component);
    }
}

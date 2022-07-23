using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcaneCollage.Entities
{
    public class Entity : IEntity
    {
        public IEnumerable<IComponent> components => _components;

        private List<IComponent> _components = new List<IComponent>();

        public Entity(params IComponent[] components)
        {
            _components.AddRange(components);
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
        }

        public void RemoveComponent(IComponent component)
        {
            _components.Remove(component);
        }
    }
}

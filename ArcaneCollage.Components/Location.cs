using ArcaneCollage.Components.Interfaces;
using ArcaneCollage.Entities.Interfaces;

namespace ArcaneCollage.Components
{
    public class Location : ILocation
    {
        private IEntity owner;

        private ISpace _space;

        public ISpace space
        {
            get
            {
                return _space;
            }
            set
            {
                if (_space != value)
                {
                    _space.OnExist(owner);
                }

                _space = value;

                _space.OnEnter(owner);
            }
        }

        public Location(IEntity owner)
        {
            this.owner = owner;
        }
    }
}

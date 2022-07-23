using System;
using System.Collections.Generic;
using System.Text;

namespace ArcaneCollage.Components.Interfaces
{
    public interface IEatPlace : ISpace
    {
        bool isFull { get; }
    }
}

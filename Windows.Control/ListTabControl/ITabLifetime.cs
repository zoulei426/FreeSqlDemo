using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Control
{
    public interface ITabLifetime
    {
        void Shown();

        void Activated();
    }
}
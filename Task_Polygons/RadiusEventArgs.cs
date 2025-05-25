using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Polygons
{
    class RadiusEventArgs : EventArgs
    {
        public int Radius { get; }
        public bool Final { get; }

        public RadiusEventArgs(int r, bool final = false)
        {
            Radius = r;
            Final = final;
        }
    }
}

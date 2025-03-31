using Avalonia.Media;
using System;

namespace Task_Polygons
{
    class ColorEventArgs : EventArgs
    {
        public Color Color { get; }

        public ColorEventArgs(Color color)
        {
            Color = color;
        }
    }
}

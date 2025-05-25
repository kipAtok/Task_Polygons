using Avalonia.Media;
using ProtoBuf;
using System.Collections.Generic;

namespace Task_Polygons
{
    [ProtoContract(SkipConstructor=true)]
    class StateInfo
    {
        [ProtoMember(8)]
        public List<Shape> Shapes { get; }

        [ProtoMember(9)]
        public int R { get; }

        [ProtoMember(10)]
        public Color Color { get; }

        public StateInfo(List<Shape> shapes, int r, Color color)
        {
            Shapes = shapes;
            R = r;
            Color = color;
        } 
    }
}

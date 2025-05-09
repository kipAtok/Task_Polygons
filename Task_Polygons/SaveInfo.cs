﻿using Avalonia.Media;
using ProtoBuf;
using System.Collections.Generic;

namespace Task_Polygons
{
    [ProtoContract(SkipConstructor=true)]
    class SaveInfo
    {
        [ProtoMember(1)]
        public List<Shape> Shapes { get; }

        [ProtoMember(2)]
        public int R { get; }

        [ProtoMember(3)]
        public Color Color { get; }

        public SaveInfo(List<Shape> shapes, int r, Color color)
        {
            Shapes = shapes;
            R = r;
            Color = color;
        } 
    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Generic;

namespace Task_Polygons
{
    public class GraphControl : UserControl
    {
        private bool _marking;
        private int _topLeftX, _topLeftY, _width, _height;
        private int _countStep, _graphStep;
        private int _jarvisCount;
        private Point[] _jarvisPoints;

        public GraphControl()
        {
            AlgTime.JarvisTime(3);

            _topLeftX = 50;
            _topLeftY = 50;
            _width = 1000;
            _height = 400;

            _countStep = 50;
            _graphStep = 50;

            _jarvisCount = 15;

            MakeGraph();
        }

        public override void Render(DrawingContext drawingContext)
        {
            DrawGraph(drawingContext);
        }

        public void RefreshGraph()
        {
            MakeGraph();
            InvalidateVisual();
        }

        public void SwitchMarking()
        {
            _marking = !_marking;
            InvalidateVisual();
        }

        private void MakeGraph()
        {
            Point[] jarvisPoints = new Point[_jarvisCount + 1];

            int jarvisX, jarvisY;

            for (int i = 1; i < _jarvisCount + 1; i++)
            {
                jarvisX = _topLeftX + 1 + _graphStep * i;
                jarvisY = _topLeftY + _height - 1 - AlgTime.JarvisTime((i + 1) * _countStep) / 10;

                jarvisPoints[i] = new Point(jarvisX, jarvisY);
            }
            jarvisPoints[0] = new Point(_topLeftX + 1, _topLeftY + _height - 1);

            _jarvisPoints = jarvisPoints;
        }

        private void DrawGraph(DrawingContext drawingContext)
        {
            Pen linePen = new Pen(new SolidColorBrush(Colors.White));
            Pen jarvisPen = new Pen(new SolidColorBrush(Colors.Yellow));

            if (_marking)
            {
                foreach (var point in _jarvisPoints)
                {
                    drawingContext.DrawLine(linePen, new Point(point.X, _topLeftY), new Point(point.X, _topLeftY + _height));
                }
            }
            else
            {
                drawingContext.DrawLine(linePen, new Point(_topLeftX, _topLeftY), new Point(_topLeftX, _topLeftY + _height));
            }

            drawingContext.DrawLine(linePen, new Point(_topLeftX, _topLeftY + _height), new Point(_topLeftX + _width, _topLeftY + _height));

            Geometry jarvisGeometry = new PolylineGeometry(_jarvisPoints, false);

            drawingContext.DrawGeometry(null, jarvisPen, jarvisGeometry);
        }
    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Diagnostics;

namespace Task_Polygons
{
    public class GraphControl: UserControl
    {
        private bool _marking;
        private int _topLeftX, _topLeftY, _width, _height;
        private int _step;
        private int _defenitionCount, _jarvisCount;
        private Point[] _defenitionPoints, _jarvisPoints;

        public GraphControl()
        {
            AlgTime.DefenitionTime(0);

            _topLeftX = 50;
            _topLeftY = 50;
            _width = 600;
            _height = 600;

            _step = 10;

            _defenitionCount = 0;
            for (int i = 0; AlgTime.DefenitionTime(3 + i * _step) / 100 < _height; i++)
            {
                _defenitionCount++;
            }
            _defenitionCount = Math.Min(_defenitionCount, _width / _step);

            _jarvisCount = _width / _step;

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
            int minCount = Math.Min(_defenitionCount, _jarvisCount);

            Point[] defenitionPoints = new Point[_defenitionCount];
            Point[] jarvisPoints = new Point[_jarvisCount];

            int defenitionX, defenitionY, jarvisX, jarvisY;

            for (int shapeCount = 3; shapeCount < minCount * _step; shapeCount += _step)
            {
                defenitionX = _topLeftX - 3 + shapeCount;
                defenitionY = _topLeftY + _height - AlgTime.DefenitionTime(shapeCount) / 100;

                jarvisX = _topLeftX - 3 + shapeCount;
                jarvisY = _topLeftY + _height - AlgTime.JarvisTime(shapeCount) / 100;

                defenitionPoints[(shapeCount - 3) / _step] = new Point(defenitionX, defenitionY);
                jarvisPoints[(shapeCount - 3) / _step] = new Point(jarvisX, jarvisY);
            }

            if (minCount == _defenitionCount)
            {
                for (int shapeCount = 3 + _defenitionCount * _step; shapeCount < _jarvisCount * _step; shapeCount += _step) 
                {
                    jarvisX = _topLeftX - 3 + shapeCount;
                    jarvisY = _topLeftY + _height - AlgTime.JarvisTime(shapeCount) / 100;

                    jarvisPoints[(shapeCount - 3) / _step] = new Point(jarvisX, jarvisY);
                }
            }
            else
            {
                for (int shapeCount = 3 + _jarvisCount * _step; shapeCount < _defenitionCount * _step; shapeCount += _step)
                {
                    defenitionX = _topLeftX - 3 + shapeCount;
                    defenitionY = _topLeftY + _height - AlgTime.DefenitionTime(shapeCount) / 100;

                    defenitionPoints[(shapeCount - 3) / _step] = new Point(defenitionX, defenitionY);
                }
            }

            _defenitionPoints = defenitionPoints;
            _jarvisPoints = jarvisPoints;
        }

        private void DrawGraph(DrawingContext drawingContext)
        {
            Pen linePen = new Pen(new SolidColorBrush(Colors.White));
            Pen defenitionPen = new Pen(new SolidColorBrush(Colors.Red));
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

            Geometry defenitionGeometry = new PolylineGeometry(_defenitionPoints, false);
            Geometry jarvisGeometry = new PolylineGeometry(_jarvisPoints, false);

            drawingContext.DrawGeometry(null, defenitionPen, defenitionGeometry);
            drawingContext.DrawGeometry(null, jarvisPen, jarvisGeometry);
        }
    }
}

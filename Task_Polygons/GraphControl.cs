using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Task_Polygons
{
    public class GraphControl: UserControl
    {
        Point[] _defenitionPoints, _jarvisPoints;

        public GraphControl()
        {
            AlgTime.DefenitionTime(0);
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

        private void MakeGraph()
        {
            Point[] defenitionPoints = new Point[6];
            Point[] jarvisPoints = new Point[52];

            int defenitionX, defenitionY, jarvisX, jarvisY;

            for (int shapeCount = 3; shapeCount < 150; shapeCount += 25)
            {
                jarvisX = 60 + shapeCount;
                jarvisY = 600 - AlgTime.JarvisTime(shapeCount) / 100;

                defenitionX = 60 + shapeCount;
                defenitionY = 600 - AlgTime.DefenitionTime(shapeCount) / 100;

                defenitionPoints[(shapeCount - 3) / 25] = new Point(defenitionX, defenitionY);
                jarvisPoints[(shapeCount - 3) / 25] = new Point(jarvisX, jarvisY);
            }

            for (int shapeCount = 153; shapeCount < 1300; shapeCount += 25)
            {
                jarvisX = 60 + shapeCount;
                jarvisY = 600 - AlgTime.JarvisTime(shapeCount) / 100;

                jarvisPoints[(shapeCount - 3) / 25] = new Point(jarvisX, jarvisY);
            }            

            _defenitionPoints = defenitionPoints;
            _jarvisPoints = jarvisPoints;
        }

        private void DrawGraph(DrawingContext drawingContext)
        {
            Pen defenitionPen = new Pen(new SolidColorBrush(Colors.Red));
            Pen jarvisPen = new Pen(new SolidColorBrush(Colors.Yellow));

            drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White)), new Point(50, 610), new Point(50, 160));
            drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White)), new Point(50, 610), new Point(1375, 610));

            Geometry defenitionGeometry = new PolylineGeometry(_defenitionPoints, false);
            Geometry jarvisGeometry = new PolylineGeometry(_jarvisPoints, false);

            drawingContext.DrawGeometry(null, defenitionPen, defenitionGeometry);
            drawingContext.DrawGeometry(null, jarvisPen, jarvisGeometry);
        }
    }
}

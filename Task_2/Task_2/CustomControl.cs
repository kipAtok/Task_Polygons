using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;

namespace Task_2
{
    public class CustomControl : UserControl
    {
        public override void Render(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Brushes.Green, 1);
            Brush brush = new SolidColorBrush(Colors.Black);

            drawingContext.DrawEllipse(brush, pen, new Point(100, 100), 10, 20);

            Console.WriteLine("DRAWING");
        }
    }
}

﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;

namespace Task_Polygons
{
    public class CustomControl : UserControl
    {
        private delegate void DrawShellDelegate(DrawingContext drawingContext);

        private int _x, _y;

        private string _shapeType;

        private Pen _pen = new Pen(Brushes.Green);
        private DrawShellDelegate DrawShell;

        private List<Shape> _shapes = [];

        private Random _random = new Random();
        private Timer _timer;

        private int _stateIndex = 0;
        private List<StateInfo> _states = [new StateInfo([], 25, Colors.Green)];
        private bool _startedMoving;

        public override void Render(DrawingContext drawingContext)
        {
            if (_shapes.Count >= 3)
            {
                DrawShell(drawingContext);
            }

            foreach (var shape in _shapes)
            {
                shape.Draw(drawingContext);
            }
        }

        public void LeftClick(int x, int y)
        {
            bool isInside = false;

            foreach (var shape in _shapes)
            {
                if (shape.IsInside(x, y))
                {
                    isInside = true;
                    shape.IsMoving = true;
                }
            }

            if (!isInside) 
            {
                AddShape(x, y);
            }
            else
            {
                AddState();
                _startedMoving = true;
                Debug.WriteLine("Movement started");
            }
            
            if (_shapes.Count >= 3)
            {
                DrawShell(null);

                if (!_shapes.Last().IsShell)
                {
                    foreach (var shape in _shapes)
                    {
                        shape.IsMoving = true;
                    }

                    RemoveNonShell();

                    AddState();
                    _startedMoving = true;
                    Debug.WriteLine("Movement started");
                }
                else if (!_startedMoving)
                {
                    RemoveNonShell();
                    AddState();
                }
            }

            _x = x;
            _y = y;

            InvalidateVisual();
        }

        public void RightClick(int x, int y)
        {
            Shape shapeToRemove = null;

            foreach (var shape in _shapes)
            {
                if (shape.IsInside(x, y))
                {
                    shapeToRemove = shape;
                }
            }

            if (shapeToRemove != null)
            {
                _shapes.Remove(shapeToRemove);

                AddState();
            }

            InvalidateVisual();
        }

        public void Move(int x, int y)
        {
            foreach (var shape in _shapes)
            {
                if (shape.IsMoving)
                {
                    shape.Move(x - _x, y - _y);
                }
            }

            _x = x;
            _y = y;

            InvalidateVisual();
        }

        public void Release(int x, int y)
        {
            foreach (var shape in _shapes)
            {
                if (shape.IsMoving)
                {
                    shape.IsMoving = false;
                }
            }

            _x = x;
            _y = y;
            
            RemoveNonShell();

            if (_startedMoving)
            {
                _startedMoving = false;
                AddState(true);
                Debug.WriteLine("Movement stoped");
            }

            InvalidateVisual();
        }

        public void ChangeShapeType(string shapeType)
        {
            _shapeType = shapeType;
        }

        public void ChangeDrawShellAlg(string drawShellAlg)
        {
            if (drawShellAlg == "Defenition")
            {
                DrawShell = new DrawShellDelegate(DrawShellDefenition);
            }
            else if (drawShellAlg == "Jarvis")
            {
                DrawShell = new DrawShellDelegate(DrawShellJarvis);
            }
        }

        public void UpdateRadius(int r, bool final = false)
        {
            Shape.R = r;

            if (final)
            {
                AddState();
            }

            InvalidateVisual();
        }

        public void UpdateColor(Color color, bool fromSettings = false)
        {
            Shape.Color = color;
            _pen = new Pen(new SolidColorBrush(color));

            if (fromSettings)
            {
                AddState();
            }

            InvalidateVisual();
        }

        public void SaveState(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);

            StateInfo state = new StateInfo(_shapes, Shape.R, Shape.Color);

            Serializer.Serialize(fs, state);

            fs.Close();
        }

        public void LoadState(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            StateInfo state = Serializer.Deserialize<StateInfo>(fs);

            ApplyState(state);

            fs.Close();
        }

        public void Clear()
        {
            _shapes = new List<Shape>();

            UpdateRadius(25);
            UpdateColor(Colors.Green);
        }

        public void StartDynamics()
        {
            if (_timer == null)
            {
                _timer = new Timer(100);
                _timer.Elapsed += Tick;
            }

            _timer.Start();
        }

        public void StopDynamics()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        public void Undo()
        {
            _stateIndex--;

            if (_stateIndex < 0)
            {
                _stateIndex = 0;
            }

            ApplyState(_states[_stateIndex]);

            Debug.WriteLine($"Undo {_stateIndex} / {_states.Count - 1}");
        }

        public void Redo()
        {
            _stateIndex++;

            if (_stateIndex == _states.Count)
            {
                _stateIndex--;
            }

            ApplyState(_states[_stateIndex]);

            Debug.WriteLine($"Redo {_stateIndex} / {_states.Count - 1}");
        }

        private void ApplyState(StateInfo state)
        {
            _shapes = state.Shapes;

            if (_shapes == null)
            {
                _shapes = new List<Shape>();
            }

            UpdateRadius(state.R);
            UpdateColor(state.Color);
        }

        private void AddState(bool lastStateIsMovement = false)
        {
            if (_stateIndex != _states.Count - 1)
            {
                if (_stateIndex == 0)
                {
                    _states = [new StateInfo([], 25, Colors.Green)];
                }
                else
                {
                    _states = _states.Slice(0, _stateIndex + 1);
                }
            }

            if (lastStateIsMovement)
            {
                _stateIndex--;
                _states = _states.Slice(0, _stateIndex + 1);
            }

            _stateIndex++;

            List<Shape> shapes = [];

            foreach (var shape in _shapes)
            {
                shapes.Add(shape.Clone());
            }

            _states.Add(new StateInfo(shapes, Shape.R, Shape.Color));

            Debug.WriteLine($"State added; Count: {_states.Count}");
        }

        private void AddShape(int x, int y)
        {
            if (_shapeType == "Circle")
            {
                _shapes.Add(new Circle(x, y));
            }
            else if (_shapeType == "Square")
            {
                _shapes.Add(new Square(x, y));
            }
            else if (_shapeType == "Triangle")
            {
                _shapes.Add(new Triangle(x, y));
            }

            if (_shapes.Count < 3)
            {
                AddState();
            }
        }

        private void DrawShellDefenition(DrawingContext drawingContext)
        {
            int i1 = 0, i2, i3;
            double k, b;
            bool above, below;

            foreach (var shape in _shapes)
            {
                shape.IsShell = false;
            }

            foreach (var shape1 in _shapes)
            {
                i2 = 0;

                foreach (var shape2 in _shapes)
                {
                    if (i2 <= i1)
                    {
                        i2++;
                        continue;
                    }

                    i3 = 0;
                    above = false;
                    below = false;

                    foreach (var shape3 in _shapes)
                    {
                        if (i3 != i1 && i3 != i2)
                        {
                            if (shape1.X != shape2.X)
                            {
                                k = (double)(shape2.Y - shape1.Y) / (shape2.X - shape1.X);
                                b = shape1.Y - shape1.X * k;

                                if (shape3.Y < k * shape3.X + b)
                                {
                                    above = true;
                                }
                                else if (shape3.Y > k * shape3.X + b)
                                {
                                    below = true;
                                }
                            }
                            else
                            {
                                if (shape3.X < shape1.X)
                                {
                                    above = true;
                                }
                                else if (shape3.X > shape1.X)
                                {
                                    below = true;
                                }
                            }
                        }

                        i3++;
                    }
                    if (above != below)
                    {
                        shape1.IsShell = true;
                        shape2.IsShell = true;

                        if (drawingContext != null)
                        {
                            drawingContext.DrawLine(_pen, new Point(shape1.X, shape1.Y), new Point(shape2.X, shape2.Y));
                        }
                    }

                    i2++;
                }

                i1++;
            }
        }

        private double Cos(Shape shape1, Shape shape2, Shape shape3)
        {
            int x1 = shape1.X, y1 = shape1.Y,
                x2 = shape2.X, y2 = shape2.Y,
                x3 = shape3.X, y3 = shape3.Y;

            double length12 = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)),
                   length23 = Math.Sqrt(Math.Pow(x2 - x3, 2) + Math.Pow(y2 - y3, 2)),
                   length31 = Math.Sqrt(Math.Pow(x3 - x1, 2) + Math.Pow(y3 - y1, 2));

            return (Math.Pow(length31, 2) - Math.Pow(length12, 2) - Math.Pow(length23, 2)) / (-2 * length12 * length23);
        }

        private void DrawShellJarvis(DrawingContext drawingContext)
        {
            Shape firstShape = _shapes.OrderBy(x => x.Y).ThenBy(x => x.X).Last();

            Shape curShape = firstShape, prevShape = new Circle(firstShape.X + 100, firstShape.Y), tempShape;
            double minCos, cos;

            foreach (var shape in _shapes)
            {
                shape.IsShell = false;
            }

            while (true)
            {
                minCos = 1;
                tempShape = null;

                foreach (var shape in _shapes)
                {
                    if (shape != prevShape)
                    {
                        cos = Cos(shape, curShape, prevShape);

                        if (cos < minCos)
                        {
                            minCos = cos;
                            tempShape = shape;
                        }
                    }
                }

                prevShape = curShape;
                prevShape.IsShell = true;
                curShape = tempShape;
                
                if (drawingContext != null)
                {
                    drawingContext.DrawLine(_pen, new Point(prevShape.X, prevShape.Y), new Point(curShape.X, curShape.Y));
                }

                if (curShape == firstShape)
                {
                    break;
                }
            }
        }

        private void RemoveNonShell()
        {
            _shapes = _shapes.FindAll(x => x.IsShell);
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            foreach (var shape in _shapes)
            {
                shape.Move(_random.Next(-1, 2), _random.Next(-1, 2));
            }

            if (_shapes.Count >= 3)
            {
                DrawShell(null);
                RemoveNonShell();
            }

            Dispatcher.UIThread.Invoke(InvalidateVisual);
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rpn.Logic
{
    static class PointExtensions
    {
        public static Point ToMathCoordinates(this Point point, Canvas canvas, double scale)
        {
            return new Point(
                (point.X - canvas.ActualWidth / 2) / scale,
                (canvas.ActualHeight / 2 - point.Y) / scale);
        }
        public static Point ToUiCoordinates(this Point point, Canvas canvas, double scale)
        {
            return new Point(
                (point.X * scale + canvas.ActualWidth / 2),
                (canvas.ActualHeight / 2 - point.Y * scale)
            );
        }
    }

    public class CanvasDrawer
    {
        private Canvas canvas;
        private double start;
        private double end;
        private double step;
        private double scale;
        private Label lblCoordinateUi;
        private Label lblCoordinateMath;

        public CanvasDrawer(Canvas canvas, Label lblCoordinateUi, Label lblCoordinateMath, double start, double end, double step, double scale)
        {
            this.canvas = canvas;
            this.lblCoordinateUi = lblCoordinateUi;
            this.lblCoordinateMath = lblCoordinateMath;
            this.start = start;
            this.end = end;
            this.step = step;
            this.scale = scale;
        }

        public void DrawAxesAndGrid()
        {
            DrawAxis(0, canvas.Height / 2, canvas.Width, canvas.Height / 2, Brushes.Black, 2);
            DrawAxis(canvas.Width / 2, 0, canvas.Width / 2, canvas.Height, Brushes.Black, 2);

            DrawTicks();

            DrawArrow(canvas.Width - 10, canvas.Height / 2, canvas.Width, canvas.Height / 2, 2);
            DrawArrow(canvas.Width / 2, 10, canvas.Width / 2, 0, 2);
        }

        private void DrawAxis(double x1, double y1, double x2, double y2, Brush color, double thickness)
        {
            Line axis = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = color,
                StrokeThickness = thickness
            };
            canvas.Children.Add(axis);
        }

        private void DrawTicks()
        {
            for (double x = start; x <= end; x += step)
            {
                double xPos = x * scale + canvas.Width / 2;
                Line tick = new Line
                {
                    X1 = xPos,
                    Y1 = canvas.Height / 2 - 5,
                    X2 = xPos,
                    Y2 = canvas.Height / 2 + 5,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                canvas.Children.Add(tick);
            }

            for (double y = start; y <= end; y += step)
            {
                double yPos = canvas.Height / 2 - y * scale;
                Line tick = new Line
                {
                    X1 = canvas.Width / 2 - 5,
                    Y1 = yPos,
                    X2 = canvas.Width / 2 + 5,
                    Y2 = yPos,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                canvas.Children.Add(tick);
            }
        }

        private void DrawArrow(double x1, double y1, double x2, double y2, double thickness)
        {
            Line arrow = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = thickness
            };
            canvas.Children.Add(arrow);

            double arrowLength = 10;
            double arrowAngle = Math.Atan2(y2 - y1, x2 - x1);

            Line arrowTip1 = new Line
            {
                X1 = x2,
                Y1 = y2,
                X2 = x2 - arrowLength * Math.Cos(arrowAngle - Math.PI / 6),
                Y2 = y2 - arrowLength * Math.Sin(arrowAngle - Math.PI / 6),
                Stroke = Brushes.Black,
                StrokeThickness = thickness
            };
            canvas.Children.Add(arrowTip1);

            Line arrowTip2 = new Line
            {
                X1 = x2,
                Y1 = y2,
                X2 = x2 - arrowLength * Math.Cos(arrowAngle + Math.PI / 6),
                Y2 = y2 - arrowLength * Math.Sin(arrowAngle + Math.PI / 6),
                Stroke = Brushes.Black,
                StrokeThickness = thickness
            };
            canvas.Children.Add(arrowTip2);
        }

        public void DrawFunction(Func<double, double> function)
        {
            double minX = start;
            double maxX = end;
            double stepX = step;

            Point prevPoint = new Point();
            bool isFirstPoint = true;

            for (double x = minX; x <= maxX; x += stepX)
            {
                double y = function(x);
                Point uiPoint = new Point(x * scale + canvas.Width / 2, canvas.Height / 2 - y * scale);

                Ellipse point = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = Brushes.Red,
                    Margin = new Thickness(uiPoint.X - 4, uiPoint.Y - 4, 0, 0)
                };
                canvas.Children.Add(point);

                if (!isFirstPoint)
                {
                    Line line = new Line
                    {
                        X1 = prevPoint.X,
                        Y1 = prevPoint.Y,
                        X2 = uiPoint.X,
                        Y2 = uiPoint.Y,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 2
                    };
                    canvas.Children.Add(line);
                }
                else
                {
                    isFirstPoint = false;
                }

                prevPoint = uiPoint;
                canvas.MouseMove += Canvas_MouseMove;
            }
        }
        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(canvas);
            Point mathPoint = mousePosition.ToMathCoordinates(canvas, scale);
            Point uiPoint = mousePosition.ToUiCoordinates(canvas, scale);

            lblCoordinateUi.Content = $"X: {uiPoint.X:F2}, Y: {uiPoint.Y:F2}";
            lblCoordinateMath.Content = $"X: {mathPoint.X:F2}, Y: {mathPoint.Y:F2}";
        }
    }
}

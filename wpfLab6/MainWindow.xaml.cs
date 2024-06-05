using System;
using System.Windows;
using Rpn.Logic;

namespace wpfLab6
{
    public partial class MainWindow : Window
    {
        private CanvasDrawer canvasDrawer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DrawGraph();
        }

        private void DrawGraph()
        {
            string input = tbInput.Text;
            double start = double.Parse(tbInputStar.Text);
            double end = double.Parse(tbInputEnd.Text);
            double step = double.Parse(tbStep.Text);
            double scale = double.Parse(tbScale.Text);

            canvasDrawer = new CanvasDrawer(CanvasGraph, lblCoordinateUi, lblCoordinateMath, start, end, step, scale);
            CanvasGraph.Children.Clear();
            canvasDrawer.DrawAxesAndGrid();
            canvasDrawer.DrawFunction(x => new RpnCalculator(input).Calculate(x));
        }
    }
}

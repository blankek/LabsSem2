using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rpn.Logic;
namespace wpfLab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double xValue = 0;
            if (tbInputX.Text.Trim() != "")
            {
                xValue = double.Parse(tbInputX.Text);
            }

            string expression = tbInput.Text;
            RpnCalculator calculator = new RpnCalculator(expression, xValue);
            ResultTextBox.Text = calculator.Result.ToString();         
        }
    }
}

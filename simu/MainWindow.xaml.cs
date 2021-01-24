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

namespace simu
{
    public partial class MainWindow : Window
    {
        InterCrack interCrack;
        public MainWindow()
        {
            InitializeComponent();
            Type.ItemsSource = new List<String>() {"MLO", "Direct Attach", "Interposer", "Hybrid", "Space Transformer" };
            Department.ItemsSource = new List<String>() {"Soldering", "Board BE", "PC Testing" };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            interCrack = new InterCrack(Double.Parse(Components.Text));
            interCrack.calculateValues(Type.Text, InterCost.Text);

            switch (Department.SelectedItem.ToString())
            {
                case "Soldering": CrackCost.Text = interCrack.interCrackValues.solderingModel.ToString(); break;
                case "Board BE": CrackCost.Text = interCrack.interCrackValues.boardModel.ToString(); break;
                case "PC Testing": CrackCost.Text = interCrack.interCrackValues.pcModel.ToString(); break;
                default: break;
            }
        }
    }
}

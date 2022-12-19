using P03AplikacjaPogoda.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace P03AplikacjaPogoda
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

        private void btnWczytajPogode_Click(object sender, RoutedEventArgs e)
        {
            string miasto = txtMiasto.Text;

            string[] miasta= miasto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            PogodaManager mp = new PogodaManager();

            foreach (var m in miasta)
            {
                lblKomunikaty.Content += $"Procesuję miasto: {m} \n";
                int temp = mp.PodajTemp(m);
                lbWynik.Items.Add($"temperatura w mieście {m} wynosi {temp}");
            }           
        }

         
    }
}

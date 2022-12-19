using P03AplikacjaPogoda.Tools;
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

            PogodaManager mp = new PogodaManager();
            int temp =mp.PodajTemp(miasto);


            lbWynik.Items.Add(temp);
        }

         
    }
}

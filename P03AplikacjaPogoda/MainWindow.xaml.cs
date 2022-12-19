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

        // Scenariusz 1 wywołanie asynchroniczne z czekaniem 
        private async void btnWczytajPogode_Click(object sender, RoutedEventArgs e)
        {
            string miasto = txtMiasto.Text;

            string[] miasta= miasto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            PogodaManager mp = new PogodaManager();

            var t= await Task.Run<int>(() =>
            {
                int temp = mp.PodajTemp(miasta[0]);
                return temp;
            });

            // to ponizej wynikona sie dopiero wtedy gdy metoda anononimowa zdefniowana 
            // powyzej wykona sie w całości. odpowiada za to słowo await 
            
            lblKomunikaty.Content += $"Procesuję miasto: {miasta[0]} \n";
            lbWynik.Items.Add($"temperatura w mieście {miasta[0]} wynosi {t}");

            //foreach (var m in miasta)
            //{
            //    lblKomunikaty.Content += $"Procesuję miasto: {m} \n";
            //    int temp = mp.PodajTemp(m);
            //    lbWynik.Items.Add($"temperatura w mieście {m} wynosi {temp}");
            //}           
        }

        // Scenariusz 2 wywołanie asynchroniczne bez czekania 
        private async void btnWczytajPogode2_Click(object sender, RoutedEventArgs e)
        {
            string miasto = txtMiasto.Text;
            string[] miasta = miasto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            PogodaManager mp = new PogodaManager();

            var t = Task.Run<int>(() =>
            {
                int temp = mp.PodajTemp(miasta[0]);
                return temp;
            });

            lblKomunikaty.Content += $"Procesuję miasto: {miasta[0]} \n";

            t.GetAwaiter().OnCompleted(() =>
            {
                lbWynik.Items.Add($"temperatura w mieście {miasta[0]} wynosi {t.Result}");
            });

            await Task.CompletedTask;
        }

        // Scenariusz 3 wywołanie asynchroniczne wiele miasta jedeno pod drugim 
        private async void btnWczytajPogode3_Click(object sender, RoutedEventArgs e)
        {
            string miasto = txtMiasto.Text;
            string[] miasta = miasto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            PogodaManager mp = new PogodaManager();

            foreach (var m in miasta)
            {
                var t = await Task.Run<int>(() =>
                {
                    int temp = mp.PodajTemp(m);
                    return temp;
                });

                lblKomunikaty.Content += $"Procesuję miasto: {m} \n";
                lbWynik.Items.Add($"temperatura w mieście {m} wynosi {t}");

            }
        }

        // Scenariusz wywołanie asynchroncze (wszystkie miasta wywoulja się niezaleznie i równolegle) ale czekam az wszystkie
        // zadania się wykonaja i dopiero po wykonaniu wszystkich zadan zwracam wynik 
        private async void btnWczytajPogode4_Click(object sender, RoutedEventArgs e)
        {
            string miasto = txtMiasto.Text;
            string[] miasta = miasto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
           // PogodaManager mp = new PogodaManager();

            List<Task> zadania = new List<Task>();

            foreach (var m in miasta)
            {
                //var t = Task.Run<object>(() =>
                //{
                //    int temp = mp.PodajTemp(m);
                //    return new { NazwaMiasta = m, Temp = temp };
                //});
                var t = Task.Run(() => PodajTemp2(m));

                zadania.Add(t);
            }
            await Task.WhenAll(zadania);

            foreach (Task<(string NazwaMiasta, int Temp)> t in zadania)
            {
                lblKomunikaty.Content += $"Procesuję miasto: {t.Result.NazwaMiasta} \n";
                lbWynik.Items.Add($"temperatura w mieście {t.Result.NazwaMiasta} wynosi {t.Result.Temp}");
            }        
        }

        private async Task<(string NazwaMiasta, int Temp)> PodajTemp2(string miasto)
        {
            PogodaManager mp = new PogodaManager();
            var temp= mp.PodajTemp(miasto);
            return await Task.FromResult((miasto, temp));
        }

        // przetwarzanie asynchroncze zwroc wynik kiedy gotowy 
        private void btnWczytajPogode5_Click(object sender, RoutedEventArgs e)
        {
            string miasto = txtMiasto.Text;
            string[] miasta = miasto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //PogodaManager mp = new PogodaManager();

            pbPostep.Maximum = miasta.Length;
            pbPostep.Value = 0;
            lblKomunikaty.Content = "";
            lbWynik.Items.Clear();

            foreach (var m in miasta)
            {
                //var t = Task.Run<int>(() =>
                //{
                //    int temp = mp.PodajTemp(m);
                //    return temp;
                //});
                var t = Task.Run(() => PodajTemperature(m));

                lblKomunikaty.Content += $"Procesuję miasto: {m} \n";

                t.GetAwaiter().OnCompleted(() =>
                {
                    lbWynik.Items.Add($"temperatura w mieście {m} wynosi {t.Result}");
                    pbPostep.Value += 1;
                });
            }

        }

        private async Task<int> PodajTemperature(string miasto)
        {
            PogodaManager mp = new PogodaManager();
            int wynik=  mp.PodajTemp(miasto);

            return await Task.FromResult(wynik);
        }

        // przetwarzanie asynchroniczne jeden po drugim 
        private async void btnWczytajPogode6_Click(object sender, RoutedEventArgs e)
        {
            lblKomunikaty.Content = "";
            lbWynik.Items.Clear();

           

            string miasto = txtMiasto.Text;
            string[] miasta = miasto.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            pbPostep.Maximum = miasta.Length;
            pbPostep.Value = 0;

            PogodaManager mp = new PogodaManager();
            
            foreach (var m in miasta)
            {
                lblKomunikaty.Content += $"Procesuję miasto: {m} \n";

                var t = await Task.Run<int>(() =>
                {
                    int temp = mp.PodajTemp(m);
                    return temp ;
                });
                
                lbWynik.Items.Add($"temperatura w mieście {m} wynosi {t}");
                pbPostep.Value += 1;
            }



        }
    }
}

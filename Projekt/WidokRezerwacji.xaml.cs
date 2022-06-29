using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Projekt.Zasoby;

namespace Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bgWorker = new BackgroundWorker();
        private int ID = 0;

        public MainWindow()
        {
            InitializeComponent();

            SchowajLadowanie();
            bgWorker.DoWork += BgWorker_wykonaj;
            bgWorker.ProgressChanged += BgWorker_zmianaProgresu;
            bgWorker.RunWorkerCompleted += BgWorker_zakonczone;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            ladowanie.Maximum = 100;
            ladowanie2.Maximum = 100;
        }
        private void TabelaGosci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabelaGosci.SelectedIndex >= 0)
            {
                if (tabelaGosci.SelectedItems.Count >= 0)
                {
                    if (tabelaGosci.SelectedItems[0].GetType() == typeof(WidokRezerwacji))
                    {
                        WidokRezerwacji dane = (WidokRezerwacji)tabelaGosci.SelectedItems[0];

                        Imie.Text = dane.Imie;
                        Nazwisko.Text = dane.Nazwisko;
                        Telefon.Text = dane.Telefon;
                        zameldowanie.SelectedDate = dane.Zameldowanie;
                        wymeldowanie.SelectedDate = dane.Wymeldowanie;
                        nrPietra.SelectedValue = dane.Pietro;
                        nrPokoju.SelectedValue = dane.Pokoj;
                        ID = dane.ID;
                    }
                }
            }
        }
        private void Pietro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> lista = new List<string>();

            switch (nrPietra.SelectedIndex)
            {
                case 0:
                    lista.AddRange(new List<string>()
                    {
                        "001",
                        "002"
                    });
                    break;
                case 1:
                    lista.AddRange(new List<string>()
                    {
                        "101",
                        "102",
                        "103",
                        "104"
                    });
                    break;
                case 2:
                    lista.AddRange(new List<string>()
                    {
                        "201",
                        "202",
                        "203",
                        "204"
                    });
                    break;
            }

            ObservableCollection<string> x = new ObservableCollection<string>(lista);
            pokoj.Visibility = Visibility.Visible;
            nrPokoju.Visibility = Visibility.Visible;
            nrPokoju.ItemsSource = null;
            nrPokoju.ItemsSource = x;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 dodajRezerwacje = new Window1();
            dodajRezerwacje.Show();
            Hide();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!bgWorker.IsBusy)
            {
                bgWorker.RunWorkerAsync();
                wczytywanie.Content = "Przerwij ładowanie";
                stan.Content = "Ładowanie...";
            }
            else
            {
                bgWorker.CancelAsync();
                wczytywanie.Content = "Wczytaj rezerwacje";
                stan.Content = "Zatrzymano!";
            }
        }
        public void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (!bgWorker.IsBusy)
            {
                bgWorker.RunWorkerAsync();
                wczytywanie2.Content = "Przerwij ładowanie";
                stan2.Content = "Ładowanie...";
            }
            else
            {
                bgWorker.CancelAsync();
                wczytywanie2.Content = "Wczytaj wolne pokoje";
                stan2.Content = "Zatrzymano!";
            }
        }
        private void SchowajLadowanie()
        {
            Dispatcher.Invoke((Action)(() =>
            {
                ladowanie.Visibility = Visibility.Hidden;
                ladowanie2.Visibility = Visibility.Hidden;
                siatka.Opacity = 1;
                siatka2.Opacity = 1;
                siatka.Effect = null;
                siatka2.Effect = null;
            }));
        }
        private void PokazLadowanie()
        {
            Dispatcher.Invoke((Action)(() =>
            {
                ladowanie.Visibility = Visibility.Visible;
                ladowanie2.Visibility = Visibility.Visible;
                siatka.Opacity = 0.5;
                siatka2.Opacity = 0.5;
                siatka.Effect = new BlurEffect();
                siatka2.Effect = new BlurEffect();
            }));
        }
        private void BgWorker_zakonczone(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                stan.Content = "Zatrzymano!";
                stan2.Content = "Zatrzymano!";
            }
            else
            {
                HotelEntities db = new HotelEntities();

                var wynik = from pob in db.Pobyt
                            join pok in db.Pokoj on pob.Rezerwacja.PokojID equals pok.ID
                            select new WidokRezerwacji
                            {
                                ID = pob.ID,
                                Pietro = pok.NumerPietra,
                                Pokoj = pok.NumerPokoju,
                                Imie = pob.Gosc.Imie,
                                Nazwisko = pob.Gosc.Nazwisko,
                                Zameldowanie = pob.Rezerwacja.DataZameldowania,
                                Wymeldowanie = pob.Rezerwacja.DataWymeldowania,
                                Telefon = pob.Gosc.Telefon,
                                Email = pob.Gosc.Email
                            };
                ObservableCollection<WidokRezerwacji> x = new ObservableCollection<WidokRezerwacji>(wynik.ToList());
                tabelaGosci.ItemsSource = x;
                tabelaGosci.HeadersVisibility = DataGridHeadersVisibility.Column;

                var wynik2 = from pokoj in db.Pokoj
                             join rezerwacja in db.Rezerwacja on pokoj.ID equals rezerwacja.PokojID
                             where pokoj.ID == rezerwacja.PokojID && SqlFunctions.DateDiff("DAY", DateTime.Now, rezerwacja.DataWymeldowania) <= 0
                             select new WidokPokoi
                             {
                                 ID = pokoj.ID,
                                 Pietro = pokoj.NumerPietra,
                                 Pokoj = pokoj.NumerPokoju,
                                 Iluosobowy = pokoj.Iluosobowy,
                                 Koszt = pokoj.Koszt
                             };
                var _wynik2 = from pokoj in db.Pokoj
                              join rezerwacja in db.Rezerwacja on pokoj.ID equals rezerwacja.PokojID
                              where pokoj.ID == rezerwacja.PokojID && SqlFunctions.DateDiff("DAY", DateTime.Now, rezerwacja.DataWymeldowania) > 0
                              select new WidokPokoi
                              {
                                  ID = pokoj.ID,
                                  Pietro = pokoj.NumerPietra,
                                  Pokoj = pokoj.NumerPokoju,
                                  Iluosobowy = pokoj.Iluosobowy,
                                  Koszt = pokoj.Koszt
                              };
                ObservableCollection<WidokPokoi> y = new ObservableCollection<WidokPokoi>(wynik2.Except(_wynik2).ToList());
                tabelaPokoi.ItemsSource = null;
                tabelaPokoi.ItemsSource = y;
                tabelaPokoi.HeadersVisibility = DataGridHeadersVisibility.Column;
            }

            stan.Content = "";
            stan2.Content = "";
            wczytywanie.Content = "Wczytaj rezerwacje";
            wczytywanie2.Content = "Wczytaj wolne pokoje";

            SchowajLadowanie();
        }
        private void BgWorker_zmianaProgresu(object sender, ProgressChangedEventArgs e)
        {
            ladowanie.Value = e.ProgressPercentage;
            ladowanie2.Value = e.ProgressPercentage;
        }
        private void BgWorker_wykonaj(object sender, DoWorkEventArgs e)
        {
            PokazLadowanie();

            for (int i = 0; i <= 100; i = i + 4)
            {
                bgWorker.ReportProgress(i);
                Thread.Sleep(25);

                if (bgWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}


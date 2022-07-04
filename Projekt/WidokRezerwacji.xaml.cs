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
        KontrolerMuzyki kM = new KontrolerMuzyki();
        private int ID = 0;
        private int walidacjaLicznik = 0;

        public MainWindow()
        {
            InitializeComponent();

            edytuj.IsEnabled = false;
            usun.IsEnabled = false;
            kM.OdtworzMuzyke();
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
        public void Button_Click_1(object sender, RoutedEventArgs e)
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
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SprawdzWalidacje();

            if (walidacjaLicznik != 2)
            {
                walidacjaLicznik = 0;
                return;
            }
            else
            {
                MessageBoxResult potwierdzenie = MessageBox.Show("Czy na pewno chcesz edytować dane gościa?", "Potwierdź edycję!", MessageBoxButton.YesNo);

                if (potwierdzenie == MessageBoxResult.Yes)
                {
                    HotelEntities db = new HotelEntities();

                    var wynikGosc = from gosc in db.Gosc
                                    join pobyt in db.Pobyt on gosc.ID equals pobyt.GoscID
                                    where gosc.ID == pobyt.GoscID && pobyt.ID == ID
                                    select gosc;
                    Gosc obiektGosc = wynikGosc.SingleOrDefault();
                    if (obiektGosc != null)
                    {
                        obiektGosc.Imie = Imie.Text;
                        obiektGosc.Nazwisko = Nazwisko.Text;
                        obiektGosc.Telefon = Telefon.Text;
                    }
                    db.SaveChanges();

                    var wynikRezerwacja = from rezerwacja in db.Rezerwacja
                                          join pokoj in db.Pokoj on rezerwacja.PokojID equals pokoj.ID
                                          join pobyt in db.Pobyt on rezerwacja.ID equals pobyt.RezerwacjaID
                                          where pobyt.ID == ID
                                          select rezerwacja;
                    Rezerwacja obiektRezerwacja = wynikRezerwacja.SingleOrDefault();
                    if (obiektRezerwacja != null)
                    {
                        obiektRezerwacja.DataZameldowania = (DateTime)zameldowanie.SelectedDate;
                        obiektRezerwacja.DataWymeldowania = (DateTime)wymeldowanie.SelectedDate;

                        switch ((string)nrPietra.SelectedValue)
                        {
                            case "0" when (string)nrPokoju.SelectedValue == "001":
                                obiektRezerwacja.PokojID = 1;
                                break;
                            case "0" when (string)nrPokoju.SelectedValue == "002":
                                obiektRezerwacja.PokojID = 2;
                                break;
                            case "1" when (string)nrPokoju.SelectedValue == "101":
                                obiektRezerwacja.PokojID = 3;
                                break;
                            case "1" when (string)nrPokoju.SelectedValue == "102":
                                obiektRezerwacja.PokojID = 4;
                                break;
                            case "1" when (string)nrPokoju.SelectedValue == "103":
                                obiektRezerwacja.PokojID = 5;
                                break;
                            case "1" when (string)nrPokoju.SelectedValue == "104":
                                obiektRezerwacja.PokojID = 6;
                                break;
                            case "2" when (string)nrPokoju.SelectedValue == "201":
                                obiektRezerwacja.PokojID = 7;
                                break;
                            case "2" when (string)nrPokoju.SelectedValue == "202":
                                obiektRezerwacja.PokojID = 8;
                                break;
                            case "2" when (string)nrPokoju.SelectedValue == "203":
                                obiektRezerwacja.PokojID = 9;
                                break;
                            case "2" when (string)nrPokoju.SelectedValue == "204":
                                obiektRezerwacja.PokojID = 10;
                                break;
                        }
                    }
                    db.SaveChanges();

                    Button_Click_1(null, null);
                }
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBoxResult potwierdzenie = MessageBox.Show("Czy na pewno chcesz usunąć dane gościa?", "Potwierdź usunięcie!", MessageBoxButton.YesNo);

            if (potwierdzenie == MessageBoxResult.Yes)
            {
                HotelEntities db = new HotelEntities();
                WidokRezerwacji pokazDane = tabelaGosci.SelectedItem as WidokRezerwacji;

                var wynikPobyt = from pobyt in db.Pobyt
                                 where pobyt.ID == ID
                                 select pobyt;
                Pobyt obiektPobyt = wynikPobyt.SingleOrDefault();

                var wynikRezerwacja = from rezerwacja in db.Rezerwacja
                                      join pobyt in db.Pobyt on rezerwacja.ID equals pobyt.RezerwacjaID
                                      where pobyt.ID == ID && rezerwacja.ID == pobyt.RezerwacjaID
                                      select rezerwacja;
                Rezerwacja obiektRezerwacja = wynikRezerwacja.SingleOrDefault();

                if (pokazDane != null)
                {
                    IList<WidokRezerwacji> dane = tabelaGosci.ItemsSource as IList<WidokRezerwacji>;

                    if (dane != null && obiektPobyt.ID > 10)
                    {
                        db.Pobyt.Remove(obiektPobyt);
                        db.Rezerwacja.Remove(obiektRezerwacja);
                        db.SaveChanges();
                        dane.Remove(pokazDane);
                    }
                    else if (dane != null && obiektPobyt.ID <= 10)
                    {
                        db.Pobyt.Remove(obiektPobyt);
                        db.SaveChanges();
                        dane.Remove(pokazDane);
                    }

                    tabelaGosci.ItemsSource = null;
                    tabelaGosci.ItemsSource = dane;
                }
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
        private void SprawdzWalidacje()
        {
            SprawdzGoscia(Imie, Nazwisko);
            SprawdzTelefon(Telefon);
        }
        private void SprawdzGoscia(TextBox poleImie, TextBox poleNazwisko)
        {
            if (!Regex.Match(poleImie.Text + poleNazwisko.Text, "^[^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\\]]{2,}$").Success)
            {
                MessageBox.Show("Pole \"" + poleImie.Name + "\" lub pole \"" + poleNazwisko.Name + "\" musi zostać poprawione!", "Nieprawidłowe dane gościa!");

                poleImie.BorderBrush = Brushes.Red;
                poleImie.BorderThickness = new Thickness(8, 0, 0, 0);
                poleImie.Padding = new Thickness(5);
                poleNazwisko.BorderBrush = Brushes.Red;
                poleNazwisko.BorderThickness = new Thickness(8, 0, 0, 0);
                poleNazwisko.Padding = new Thickness(5);
            }
            else walidacjaLicznik++;
        }
        private void SprawdzTelefon(TextBox poleTelefon)
        {
            if (!Regex.Match(poleTelefon.Text, "^[+]?[0-9 ]+$").Success)
            {
                MessageBox.Show("Pole \"" + poleTelefon.Name + "\" musi zostać poprawione!", "Niepoprawny Telefon!");

                poleTelefon.BorderBrush = Brushes.Red;
                poleTelefon.BorderThickness = new Thickness(8, 0, 0, 0);
                poleTelefon.Padding = new Thickness(5);
            }
            else walidacjaLicznik++;
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
        private void FormatDaty(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime)) (e.Column as DataGridBoundColumn).Binding.StringFormat = "dd/MM/yyyy";
            if (e.PropertyName == "Email") e.Column.MinWidth = 154;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}


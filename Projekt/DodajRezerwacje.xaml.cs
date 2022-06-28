using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Projekt
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            rezerwuj.IsEnabled = false;
        }
        private void ZmianaDanych(object sender, EventArgs e)
        {
            DateTime pierwszaData = new DateTime();
            DateTime drugaData = new DateTime();
            DateTime dzis = DateTime.Now.Date;

            if (Zameldowanie.SelectedDate != null) pierwszaData = (DateTime)Zameldowanie.SelectedDate;
            if (Wymeldowanie.SelectedDate != null) drugaData = (DateTime)Wymeldowanie.SelectedDate;

            int iloscDob = (int)drugaData.Subtract(pierwszaData).TotalDays;
            int _iloscDob = (int)dzis.Subtract(pierwszaData).TotalDays;

            if (string.IsNullOrWhiteSpace(Imie.Text) || string.IsNullOrWhiteSpace(Nazwisko.Text) ||
                string.IsNullOrWhiteSpace(Telefon.Text) || Zameldowanie.SelectedDate == null || Wymeldowanie.SelectedDate == null ||
                Pokoj.SelectedItem == null || iloscDob <= 0 || _iloscDob > 0)
                rezerwuj.IsEnabled = false;
            else rezerwuj.IsEnabled = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow widokRezerwacji = new MainWindow();
            widokRezerwacji.Show();
            Hide();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            HotelEntities db = new HotelEntities();

            Gosc gosc = new Gosc()
            {
                Imie = Imie.Text,
                Nazwisko = Nazwisko.Text,
                Kraj = Kraj.SelectedValue == null ? String.Empty : Kraj.SelectedValue.ToString(),
                Telefon = Telefon.Text,
                Email = Email.Text
            };

            int pokojID = 1;
            decimal koszt = 0;

            switch ((string)Pokoj.SelectedValue)
            {
                case "001 - Parter":
                    pokojID = 1;
                    koszt = 299.99M;
                    break;
                case "002 - Parter":
                    pokojID = 2;
                    koszt = 349.99M;
                    break;
                case "101 - Piętro 1":
                    pokojID = 3;
                    koszt = 199.99M;
                    break;
                case "102 - Piętro 1":
                    pokojID = 4;
                    koszt = 199.99M;
                    break;
                case "103 - Piętro 1":
                    pokojID = 5;
                    koszt = 224.99M;
                    break;
                case "104 - Piętro 1":
                    pokojID = 6;
                    koszt = 224.99M;
                    break;
                case "201 - Piętro 2":
                    pokojID = 7;
                    koszt = 199.99M;
                    break;
                case "202 - Piętro 2":
                    pokojID = 8;
                    koszt = 324.99M;
                    break;
                case "203 - Piętro 2":
                    pokojID = 9;
                    koszt = 224.99M;
                    break;
                case "204 - Piętro 2":
                    pokojID = 10;
                    koszt = 299.99M;
                    break;
            }

            Rezerwacja rezerwacja = new Rezerwacja()
            {
                DataZameldowania = (DateTime)Zameldowanie.SelectedDate,
                DataWymeldowania = (DateTime)Wymeldowanie.SelectedDate,
                PokojID = pokojID
            };

            int iloscDob = (int)(rezerwacja.DataWymeldowania - rezerwacja.DataZameldowania).TotalDays;
            int wynik = (from pokoj in db.Pokoj
                         join rez in db.Rezerwacja on pokoj.ID equals rezerwacja.PokojID
                         where pokoj.ID == rez.PokojID && SqlFunctions.DateDiff("DAY", DateTime.Now, rez.DataWymeldowania) > 0
                         select pokoj).Count();

            if (wynik > 0)
                MessageBox.Show("Wybrany pokój jest już zajęty! Sprawdź wykaz wolnych pokoi i wybierz ponownie.", "Nieprawidłowy pokój!");
            else
            {
                MessageBoxResult potwierdzenie = MessageBox.Show("Czy na pewno chcesz zarezerwować pokój?\nKoszt rezerwacji wyniesie: " + iloscDob * koszt + " zł", "Potwierdź rezerwację!", MessageBoxButton.YesNo);

                if (potwierdzenie == MessageBoxResult.Yes)
                {
                    db.Gosc.Add(gosc);
                    db.SaveChanges();
                    int goscID = gosc.ID;

                    db.Rezerwacja.Add(rezerwacja);
                    db.SaveChanges();
                    int rezerwacjaID = rezerwacja.ID;

                    Pobyt pobyt = new Pobyt()
                    {
                        GoscID = goscID,
                        RezerwacjaID = rezerwacjaID
                    };
                    db.Pobyt.Add(pobyt);
                    db.SaveChanges();

                    MainWindow widokRezerwacji = new MainWindow();
                    widokRezerwacji.Show();
                    Hide();
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

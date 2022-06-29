using System;

namespace Projekt.Zasoby
{
    public class WidokRezerwacji
    {
        public int ID { get; set; }
        public string Pietro { get; set; }
        public string Pokoj { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime Zameldowanie { get; set; }
        public DateTime Wymeldowanie { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }

        public static WidokRezerwacji From(Gosc gosc, Rezerwacja rezerwacja, Pokoj pokoj, Pobyt pobyt)
        {
            return new WidokRezerwacji
            {
                ID = pobyt.ID,
                Pietro = pokoj.NumerPietra,
                Pokoj = pokoj.NumerPokoju,
                Imie = gosc.Imie,
                Nazwisko = gosc.Nazwisko,
                Zameldowanie = rezerwacja.DataZameldowania,
                Wymeldowanie = rezerwacja.DataWymeldowania,
                Telefon = gosc.Telefon,
                Email = gosc.Email
            };
        }
    }
}

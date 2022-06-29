namespace Projekt.Zasoby
{
    public class WidokPokoi
    {
        public int ID { get; set; }
        public string Pietro { get; set; }
        public string Pokoj { get; set; }
        public string Iluosobowy { get; set; }
        public decimal Koszt { get; set; }

        public static WidokPokoi From(Pokoj pokoj)
        {
            return new WidokPokoi
            {
                ID = pokoj.ID,
                Pietro = pokoj.NumerPietra,
                Pokoj = pokoj.NumerPokoju,
                Iluosobowy = pokoj.Iluosobowy,
                Koszt = pokoj.Koszt
            };
        }
    }
}

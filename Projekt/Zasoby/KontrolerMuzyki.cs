namespace Projekt.Zasoby
{
    public class KontrolerMuzyki
    {
        private System.Media.SoundPlayer odtwarzacz = new System.Media.SoundPlayer(Properties.Resources.muzyka);

        public void OdtworzMuzyke()
        {
            odtwarzacz.Play();
        }
        public void PrzerwijMuzyke()
        {
            odtwarzacz.Stop();
        }
    }
}

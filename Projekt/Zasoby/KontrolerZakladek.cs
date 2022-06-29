using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Projekt.Zasoby
{
    public class KontrolerZakladek : IMultiValueConverter
    {
        public object Convert(object[] wartosci, Type typ, object parametr, CultureInfo culture)
        {
            TabControl kontrolaZakladki = wartosci[0] as TabControl;
            double szerokosc = kontrolaZakladki.ActualWidth / kontrolaZakladki.Items.Count;

            return (szerokosc <= 1) ? 0 : (szerokosc - 2);
        }
        public object[] ConvertBack(object wartosci, Type[] typy, object parametr, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
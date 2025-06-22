using System;
using System.Globalization;
using System.Windows.Data;

namespace khaiWPF
{
    public class BookingStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int status = (int)value;
            if (status == 1)
                return "�ang ho?t �?ng";
            else if (status == 2)
                return "�? h?y";
            else if (status == 3)
                return "Ho�n t?t";
            else
                return "Kh�ng x�c �?nh";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

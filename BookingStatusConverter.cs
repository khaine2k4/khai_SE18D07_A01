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
                return "Ðang ho?t ð?ng";
            else if (status == 2)
                return "Ð? h?y";
            else if (status == 3)
                return "Hoàn t?t";
            else
                return "Không xác ð?nh";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

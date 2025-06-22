using System;
using System.Globalization;
using System.Windows.Data;

namespace khaiWPF

{
    public class BookingStatusConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                switch (status)
                {
                    case 1: return "Đang hoạt động";
                    case 2: return "Đã hủy";
                    case 3: return "Hoàn tất";
                    default: return "Không xác định";
                }
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
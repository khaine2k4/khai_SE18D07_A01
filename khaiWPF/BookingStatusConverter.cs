using System;
using System.Globalization;
using System.Windows.Data;

namespace khaiWPF
{
    public class BookingStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                switch (status)
                {
                    case 0: return "Chờ xác nhận";
                    case 1: return "Đã xác nhận";
                    case 2: return "Đã hủy";
                    case 3: return "Đã hoàn thành";
                    case 4: return "Đã thanh toán";
                    default: return "Không xác định";
                }
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string statusText)
            {
                switch (statusText)
                {
                    case "Chờ xác nhận": return 0;
                    case "Đã xác nhận": return 1;
                    case "Đã hủy": return 2;
                    case "Đã hoàn thành": return 3;
                    case "Đã thanh toán": return 4;
                    default: return 0;
                }
            }
            return 0;
        }
    }
}
using System.Globalization;
using Makara.Models;

namespace Makara.Helpers;
public class StatusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is StatusType status)
            return status.ToString();
        return "Unknown";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Implement if needed; otherwise, throw.
        throw new NotImplementedException();
    }
}

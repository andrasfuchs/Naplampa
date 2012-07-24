using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows;

namespace NaplampaAdmin.Util
{
    class OrderValueConverter : IMultiValueConverter
    {
        // Summary:
        //     Converts source values to a value for the binding target. The data binding
        //     engine calls this method when it propagates the values from source bindings
        //     to the binding target.
        //
        // Parameters:
        //   values:
        //     The array of values that the source bindings in the System.Windows.Data.MultiBinding
        //     produces. The value System.Windows.DependencyProperty.UnsetValue indicates
        //     that the source binding has no value to provide for conversion.
        //
        //   targetType:
        //     The type of the binding target property.
        //
        //   parameter:
        //     The converter parameter to use.
        //
        //   culture:
        //     The culture to use in the converter.
        //
        // Returns:
        //     A converted value.  If the method returns null, the valid null value is used.
        //      A return value of System.Windows.DependencyProperty.System.Windows.DependencyProperty.UnsetValue
        //     indicates that the converter did not produce a value, and that the binding
        //     will use the System.Windows.Data.BindingBase.FallbackValue if it is available,
        //     or else will use the default value.  A return value of System.Windows.Data.Binding.System.Windows.Data.Binding.DoNothing
        //     indicates that the binding does not transfer the value or use the System.Windows.Data.BindingBase.FallbackValue
        //     or the default value.
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != DependencyProperty.UnsetValue)
            {
                DateTime created = (DateTime)values[0];
                int orderStatus = (int)values[1];
                DateTime? sentOn = (DateTime?)values[2];

                if ((orderStatus & 65536) == 65536)
                {
                    return new SolidColorBrush(Colors.Gray);
                }

                if (((orderStatus & 16) == 16) && ((orderStatus & 64) != 64) && ((orderStatus & 256) != 256) && ((orderStatus & 2048) != 2048) && (sentOn.HasValue) && (sentOn.Value.AddDays(14.0) < DateTime.Today))
                {
                    return new SolidColorBrush(Colors.LightYellow);
                }

                if (((orderStatus & 8) == 8) && ((orderStatus & 16) != 16))
                {
                    return new SolidColorBrush(Colors.LightPink);
                }

                if (((orderStatus & 3) == 3) && ((orderStatus & 4) != 4) && ((orderStatus & 8) != 8) && (created.AddDays(7.0) < DateTime.Today))
                {
                    return new SolidColorBrush(Colors.LightPink);
                }

                if (((orderStatus & 3) == 3) && ((orderStatus & 4) != 4) && ((orderStatus & 8) != 8) && (created.AddDays(3.0) < DateTime.Today))
                {
                    return new SolidColorBrush(Colors.LightYellow);
                }

                if (created.AddDays(21.0) < DateTime.Today)
                {
                    return new SolidColorBrush(Colors.LightGray);
                }
            }

            return new SolidColorBrush(Colors.White);
        }

        //
        // Summary:
        //     Converts a binding target value to the source binding values.
        //
        // Parameters:
        //   value:
        //     The value that the binding target produces.
        //
        //   targetTypes:
        //     The array of types to convert to. The array length indicates the number and
        //     types of values that are suggested for the method to return.
        //
        //   parameter:
        //     The converter parameter to use.
        //
        //   culture:
        //     The culture to use in the converter.
        //
        // Returns:
        //     An array of values that have been converted from the target value back to
        //     the source values.
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}

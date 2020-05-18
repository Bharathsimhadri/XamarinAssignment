using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XFTest.Converters
{
    public class StateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!=null)
            {
                if(value is string _state)
                {
                    if(_state == "ToDo")
                    {
                        return Color.FromHex("#4E77D6");
                    }
                    else if(_state=="InProgress")
                    {
                        return Color.FromHex("#F5709");
                    }
                    else if(_state=="Done")
                    {
                        return Color.FromHex("#25A87B");
                    }
                    else if(_state=="Rejected")
                    {
                        return Color.FromHex("#EF6565");
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

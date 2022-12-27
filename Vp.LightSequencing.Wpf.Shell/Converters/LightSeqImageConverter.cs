using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Vp.LightSequencing.Wpf.Shell.Converters
{
    /// <summary>
    /// Gets an image for the Effect name
    /// </summary>
    public class LightSeqImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            var effectName = value?.ToString().Replace("On", "").Replace("Off", "");
            effectName += "Img";
            var effect = Application.Current.TryFindResource(effectName) as BitmapImage;
            if(effect != null)
            {
                return effect;
            }
            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Thimbles_Game
{
    /// <summary>
    /// Конвертер, конвертирует путь к изображению в элемент изображения.
    /// </summary>
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string) value;
            BitmapImage bm = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            return bm;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HirportalAdmin.ViewModel
{
    class ImageModel
    {
        public static Byte[] OpenAndResize(String path, Int32 height)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            BitmapImage image = new BitmapImage(); // kép betöltése
            image.BeginInit();
            image.UriSource = new Uri(path);
            image.DecodePixelHeight = height; // megadott méretre
            image.EndInit();

            PngBitmapEncoder encoder = new PngBitmapEncoder(); // átalakítás PNG formátumra
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (MemoryStream stream = new MemoryStream()) // átalakítás byte-tömbre
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }
}

using System.Drawing;

namespace Images
{
    public static class ImageExtensions
    {
        public static ColorImage ApplyPointOperation(this ColorImage image, Func<Color, Color> pointOperation)
            => ApplyPointOperation<ColorImage, Color>(image, pointOperation);

        public static GrayscaleImage ApplyPointOperation(this GrayscaleImage image, Func<byte, byte> pointOperation)
            => ApplyPointOperation<GrayscaleImage, byte>(image, pointOperation);

        public static T ApplyPointOperation<T, U>(this GrayscaleImage image, Func<byte, U> pointOperation)
            where T : Image<U>, new()
        {
            var lookupTable = new U[256];

            for (int i = 0; i < 256; i++)
                lookupTable[i] = pointOperation((byte)i);

            T tempImage = Image<U>.CreateEmpty<T>(image.Dimensions);

            Parallel.For(0, image.PixelCount, i => tempImage[i] = lookupTable[image[i]]);

            return tempImage;
        }

        public static T ApplyPointOperation<T, U>(this BinaryImage image, Func<byte, U> pointOperation)
            where T : Image<U>, new()
        {
            U zero = pointOperation(0);
            U one = pointOperation(1);

            T tempImage = Image<U>.CreateEmpty<T>(image.Dimensions);

            Parallel.For(0, image.PixelCount, i => tempImage[i] = image[i] == 0 ? zero : one);

            return tempImage;
        }

        public static T ApplyPointOperation<T, U>(this T image, Func<U, U> pointOperation)
            where T : Image<U>, new()
            => image.ApplyPointOperation<T, U, T, U>(pointOperation);

        public static V ApplyPointOperation<T, U, V, W>(this T image, Func<U, W> pointOperation)
            where T : Image<U> where V : Image<W>, new()
        {
            V tempImage = Image<W>.CreateEmpty<V>(image.Dimensions);

            Parallel.For(0, image.PixelCount, i => tempImage[i] = pointOperation(image[i]));

            return tempImage;
        }
    }
}

using System.Drawing;

namespace Images
{
    public partial class GrayscaleImage : Image2D<byte>
    {
        /// <summary>
        /// Warning: do not use! Useful for using generic type constraint 'new()' within this and parent classes.
        /// </summary>
        public GrayscaleImage() : base() { }

        public GrayscaleImage(GrayscaleImage image) : base(image) { }

        public GrayscaleImage(int width, int height) : base(width, height) { }

        public GrayscaleImage(ColorImage image, GrayscaleConversion conversion = GrayscaleConversion.RGB)
            : base(image.Width, image.Height)
        {
            Func<Color, byte> convertToGray = GetGrayscaleConversionMethod(conversion);

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    Color pixel = image[x, y];
                    this[x, y] = convertToGray(pixel);
                }
        }

        private static Func<Color, byte> GetGrayscaleConversionMethod(GrayscaleConversion conversion)
        => conversion switch
        {
            GrayscaleConversion.RGB => (Color c) => (byte)((c.R + c.B + c.G) / 3),
            GrayscaleConversion.HSV => (Color c) => Math.Max(Math.Max(c.R, c.G), c.B),
            GrayscaleConversion.HSL => (Color c) => Math.Min(Math.Min(c.R, c.G), c.B),
            _ => throw new ArgumentOutOfRangeException(nameof(conversion)),
        };

        public override Image<byte> Copy()
            => new(this);

        public static explicit operator GrayscaleImage(BinaryImage image)
            => Cast<BinaryImage, GrayscaleImage>(image);
    }

    /// <summary>
    /// RGB takes the average, HSV takes the maximum and HSL takes the minimum of the three color channels.
    /// </summary>
    public enum GrayscaleConversion
    {
        RGB, HSV, HSL
    }
}
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Images
{
    public partial class ColorImage : Image2D<Color>
    {
        /// <summary>
        /// Warning: do not use! Useful for using generic type constraint 'new()' within this and parent classes.
        /// </summary>
        public ColorImage() : base() { }

        public ColorImage(ColorImage image) : base(image) { }

        public ColorImage(int width, int height) : base(width, height) { }

        public ColorImage(
            Image2D<byte> image)
            : base(image.Width, image.Height)
        {
            Parallel.For(0, image.PixelCount, i => _data[i] = GetColor(i));

            Color GetColor(int i)
            {
                int value = image[i];
                return Color.FromArgb(value, value, value);
            }
        }

        public ColorImage(
            Image2D<byte> redChannel, Image2D<byte> greenChannel,
            Image2D<byte> blueChannel, Image2D<byte>? alphaChannel = null)
            : base(redChannel.Width, redChannel.Height)
        {
            if (!GrayscaleImage.AreSameSize(redChannel, greenChannel, blueChannel) ||
                !(alphaChannel?.IsSameSizeAs(redChannel) ?? true))
                throw new ArgumentException($"The provided color channels in {nameof(ColorImage)} must have the same size.");

            Parallel.For(0, redChannel.PixelCount, i => _data[i] = CombineChannels(i));

            Color CombineChannels(int i)
                => Color.FromArgb(alphaChannel?[i] ?? byte.MaxValue, redChannel[i], greenChannel[i], blueChannel[i]);
        }

        public GrayscaleImage GetChannel(ColorChannel channel)
        {
            Func<int, int, byte> GetPixel = channel switch
            {
                ColorChannel.R => (x, y) => this[x, y].R,
                ColorChannel.G => (x, y) => this[x, y].G,
                ColorChannel.B => (x, y) => this[x, y].B,
                ColorChannel.A => (x, y) => this[x, y].A,
                _ => throw new ArgumentOutOfRangeException(nameof(channel)),
            };

            GrayscaleImage tempImage = new(Width, Height);

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    tempImage[x, y] = GetPixel(x, y);

            return tempImage;
        }

        public GrayscaleImage GetRedChannel()
            => GetChannel(ColorChannel.R);

        public GrayscaleImage GetGreenChannel()
            => GetChannel(ColorChannel.G);

        public GrayscaleImage GetBlueChannel()
            => GetChannel(ColorChannel.B);

        public GrayscaleImage GetAlphaChannel()
            => GetChannel(ColorChannel.A);

        public override Image<Color> Copy()
            => new(this);
    }

    public enum ColorChannel
    {
        R, G, B, A
    }
}
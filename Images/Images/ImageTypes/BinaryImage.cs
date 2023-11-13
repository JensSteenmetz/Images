namespace Images
{
    public partial class BinaryImage : Image2D<byte>
    {
        /// <summary>
        /// Warning: do not use! Useful for using generic type constraint 'new()' within this and parent classes.
        /// </summary>
        public BinaryImage() : base() { }

        public BinaryImage(BinaryImage image) : base(image) { }

        public BinaryImage(int width, int height) : base(width, height) { }

        public override byte this[int index]
        {
            get => _data[index] > byte.MinValue ? byte.MaxValue : byte.MinValue;
            set => _data[index] = value;
        }

        public override byte this[int x, int y]
        {
            get => base[x, y] > byte.MinValue ? byte.MaxValue : byte.MinValue;
            set => base[x, y] = value;
        }

        public override Image<byte> Copy()
            => new(this);

        public static explicit operator BinaryImage(GrayscaleImage image)
            => Cast<GrayscaleImage, BinaryImage>(image);
    }
}
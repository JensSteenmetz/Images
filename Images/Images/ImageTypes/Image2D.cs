namespace Images
{
    public abstract class Image2D<T> : Image<T>
    {
        public virtual int Width => Dimensions[0];
        public virtual int Height => Dimensions[1];
        public (int, int) Size => (Width, Height);

        /// <summary>
        /// Warning: do not use! Useful for using generic type constraint 'new()' within this and parent classes.
        /// </summary>
        public Image2D() : base() { }

        public Image2D(Image2D<T> image) : base(image) { }

        public Image2D(int width, int height) : base(width, height) { }

        public T this[(int x, int y) coordinates]
        {
            get => this[coordinates.x, coordinates.y];
            set => this[coordinates.x, coordinates.y] = value;
        }

        public virtual T this[int x, int y]
        {
            get => _data[GetIndex(x, y)];
            set => _data[GetIndex(x, y)] = value;
        }

        private int GetIndex(int x, int y)
            => y * Width + x;

        protected static V Cast<U, V>(U image) where U : Image2D<T> where V : Image2D<T>, new()
        {
            V tempImage = new()
            {
                Dimensions = image.Dimensions,
                NumberOfDimensions = image.NumberOfDimensions,
                PixelCount = image.PixelCount,
                _data = image._data
            };

            return tempImage;
        }

        public override Image<T> Copy()
            => new(this);

        public bool IsSameSizeAs(Image2D<T> image)
            => this.Size == image.Size;

        public static bool AreSameSize(params Image2D<T>[] images)
        {
            if (images.Length == 0) return true;

            (int, int) size = images[0].Size;

            foreach (Image2D<T> image in images)
                if (image.Size != size) return false;

            return true;
        }
    }
}
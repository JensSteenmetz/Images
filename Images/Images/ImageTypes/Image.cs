using System.Collections;

namespace Images
{
    public class Image<T> : IEnumerable<T>, IEnumerable, ICloneable
    {
        protected T[] _data;

        public int PixelCount { get; protected set; }

        public int NumberOfDimensions { get; protected set; }

        public int[] Dimensions { get; protected set; }

        /// <summary>
        /// Warning: do not use! Useful for using generic type constraint 'new()' within this and parent classes.
        /// </summary>
        public Image()
        {
            Dimensions = Array.Empty<int>();
            NumberOfDimensions = 0;
            PixelCount = 0;

            _data = Array.Empty<T>();
        }

        public Image(Image<T> image) : this(image.Dimensions)
            => Array.Copy(image._data, _data, _data.Length);

        public Image(params int[] dimensions)
        {
            Dimensions = dimensions;
            NumberOfDimensions = dimensions.Length;
            PixelCount = dimensions.Aggregate((a, b) => a * b);

            _data = new T[PixelCount];
        }

        public virtual T this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        public virtual T this[params int[] coordinates]
        {
            get => _data[GetIndex(coordinates)];
            set => _data[GetIndex(coordinates)] = value;
        }

        protected virtual int GetIndex(params int[] coordinates)
        {
            if (coordinates.Length != NumberOfDimensions)
                throw new ArgumentOutOfRangeException(nameof(coordinates));

            int sum = 0, step = 1;

            for (int i = 0; i < NumberOfDimensions; i++)
            {
                sum += step * coordinates[i];
                step *= Dimensions[i];
            }

            return sum;
        }

        public IEnumerator<T> GetEnumerator()
            => ((IEnumerable<T>)_data).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _data.GetEnumerator();

        public virtual Image<T> Copy()
            => new(this);

        public virtual object Clone()
            => Copy();

        public static U CreateEmpty<U>(params int[] dimensions) where U : Image<T>, new()
        {
            int pixelCount = dimensions.Aggregate((a, b) => a * b);

            U image = new()
            {
                Dimensions = dimensions,
                NumberOfDimensions = dimensions.Length,
                PixelCount = pixelCount,
                _data = new T[pixelCount]
            };

            return image;
        }
    }
}
namespace Images
{
    public static class ImageExtensions
    {
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

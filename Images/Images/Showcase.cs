namespace Images
{
    public class Showcase
    {
        /// <summary>
        /// This method provides some basic examples of how the various image classes can be used.
        /// </summary>
        public static void Example()
        {
            /// Create black grayscale (byte) image with a size of 1920x1080.
            GrayscaleImage blackImage = new(1920, 1080);

            /// Set the pixels in the image to random byte values by applying a (parallellized) point operation.
            GrayscaleImage randomImage = blackImage.ApplyPointOperation((Func<byte, byte>)RandomValue);

            /// Threshold the image (alternative point operation syntax).
            BinaryImage thresholded = (BinaryImage)randomImage.ApplyPointOperation<GrayscaleImage, byte>(Threshold);

            /// Explicitly cast the original grayscale image to binary. 
            /// The underlying pixel data is shared between 'randomImage' and 'casted', 
            /// but accessing the pixel values will now return either 0 or 255.
            BinaryImage casted = (BinaryImage)randomImage;

            /// Create a (black-and-white) color image from the binary image.
            ColorImage colorImage = new(casted);

            /// Create a 3D float image with a size of 1920x1080x100.
            Image<float> floatImage3D = new(1920, 1080, 100);

            /// Floor the values in the float image (point operation changing the pixel type).
            Image<int> integerImage3D = floatImage3D.ApplyPointOperation<Image<float>, float, Image<int>, int>(Floor);

            static byte Threshold(byte b) => b > 127 ? byte.MaxValue : byte.MinValue;
            static byte RandomValue(byte _) => (byte)Random.Shared.Next(0, 256);
            static int Floor(float x) => (int)x;
        }
    }
}

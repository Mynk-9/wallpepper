using Windows.UI.Xaml.Media.Imaging;

namespace wallpepper.Views
{
    static class WallpaperHandler
    {
        private static BitmapImage SpotlightImage;
        private static BitmapImage BingImage;

        public static bool isSpotlightImageLoaded { get; private set; } = false;
        public static bool isBingImageLoaded { get; private set; } = false;

        public static void setSpotlightImage(BitmapImage img)
        {
            SpotlightImage = img;
            isSpotlightImageLoaded = true;
        }
        public static void setBingImage(BitmapImage img)
        {
            SpotlightImage = img;
            isBingImageLoaded = true;
        }
    }
}

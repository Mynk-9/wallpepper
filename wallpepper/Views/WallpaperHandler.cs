using Windows.UI.Xaml.Media.Imaging;

namespace wallpepper.Views
{
    class WallpaperHandler
    {
        public static BitmapImage SpotlightImage { get; private set; }
        public static BitmapImage BingImage { get; private set; }

        public static bool isSpotlightImageLoaded { get; private set; } = false;
        public static bool isBingImageLoaded { get; private set; } = false;

        public static void setSpotlightImage(BitmapImage img)
        {
            SpotlightImage = img;
            isSpotlightImageLoaded = true;
        }
        public static void setBingImage(BitmapImage img)
        {
            BingImage = img;
            isBingImageLoaded = true;
        }
    }
}

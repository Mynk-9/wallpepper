using Windows.UI.Xaml.Media.Imaging;
namespace wallpepper.Views
{
    static class WallpaperHandler
    {
        public static BitmapImage SpotlightImage { get; private set; }
        public static BitmapImage BingImage { get; private set; }

        public static bool IsSpotlightImageLoaded { get; private set; } = false;
        public static bool IsBingImageLoaded { get; private set; } = false;

        public static void SetSpotlightImage(BitmapImage img)
        {
            SpotlightImage = img;
            IsSpotlightImageLoaded = true;
        }
        public static void SetBingImage(BitmapImage img)
        {
            BingImage = img;
            IsBingImageLoaded = true;
        }
    }
}

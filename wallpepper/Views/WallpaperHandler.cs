using Windows.UI.Xaml.Media.Imaging;
namespace wallpepper.Views
{
    static class WallpaperHandler
    {
        public static SoftwareBitmapSource SpotlightImage { get; private set; }
        public static SoftwareBitmapSource BingImage { get; private set; }

        public static bool IsSpotlightImageLoaded { get; private set; } = false;
        public static bool IsBingImageLoaded { get; private set; } = false;

        public static void SetSpotlightImage(SoftwareBitmapSource img)
        {
            SpotlightImage = img;
            IsSpotlightImageLoaded = true;
        }
        public static void SetBingImage(SoftwareBitmapSource img)
        {
            BingImage = img;
            IsBingImageLoaded = true;
        }
    }
}

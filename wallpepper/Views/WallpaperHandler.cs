using Windows.Graphics.Imaging;
namespace wallpepper.Views
{
    static class WallpaperHandler
    {
        public static SoftwareBitmap SpotlightImage { get; private set; }
        public static SoftwareBitmap BingImage { get; private set; }

        public static bool IsSpotlightImageLoaded { get; private set; } = false;
        public static bool IsBingImageLoaded { get; private set; } = false;

        public static void SetSpotlightImage(SoftwareBitmap img)
        {
            SpotlightImage = img;
            IsSpotlightImageLoaded = true;
        }
        public static void SetBingImage(SoftwareBitmap img)
        {
            BingImage = img;
            IsBingImageLoaded = true;
        }
    }
}

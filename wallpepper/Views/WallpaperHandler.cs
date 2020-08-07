using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.System.UserProfile;
namespace wallpepper.Views
{
    static class WallpaperHandler
    {
        public static SoftwareBitmap SpotlightImage { get; private set; }
        public static bool IsSpotlightImageLoaded { get; private set; } = false;
        public static bool IsSpotLightImageSavedToGallery { get; set; } = false;

        public static SoftwareBitmap BingImage { get; private set; }
        public static bool IsBingImageLoaded { get; private set; } = false;
        public static bool IsBingImageSavedToGallery { get; set; } = false;

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

    static class DesktopWallpaper
    {
        // The following commented lines are saved for future purpose if we need
        // to set wallpaper on devices not supporting UserProfilePersonalizationSettings
        /**
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        private static extern int SystemParametersInfo(uint uAction, int uParam, StringBuilder lpvParam, uint fuWinIni);

        private enum WallpaperAction
        {
            SPI_SETDESKWALLPAPER = 0x0014,
            SPIF_UPDATEINIFILE = 0x0001
        }
        */

        public static async Task<bool> SetDesktopBackgroundWallpaper(string fileUri)
        {
            if (!UserProfilePersonalizationSettings.IsSupported())
                return false;

            UserProfilePersonalizationSettings personalizationSettings = UserProfilePersonalizationSettings.Current;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(fileUri));
            return await personalizationSettings.TrySetWallpaperImageAsync(file);
        }
    }
}

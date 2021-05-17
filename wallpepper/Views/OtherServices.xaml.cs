using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace wallpepper.Views
{
    public sealed partial class OtherServices : Page
    {
        private string bingImageURL, spotlightImageURL;
        public OtherServices()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (WallpaperHandler.IsBingImageSavedToGallery)
                bingSaveToGallery.IsEnabled = false;
            if (WallpaperHandler.IsSpotLightImageSavedToGallery)
                spotlightSaveToGallery.IsEnabled = false;

            SoftwareBitmap bingImg, spotlightImg;
            if (WallpaperHandler.IsBingImageLoaded == false)
            {
                bingImg = await GetBingImage();
                WallpaperHandler.SetBingImage(bingImg);
            }
            SetBingImage(WallpaperHandler.BingImage);
            if (WallpaperHandler.IsSpotlightImageLoaded == false)
            {
                spotlightImg = await GetSpotlightImage();
                WallpaperHandler.SetSpotlightImage(spotlightImg);
            }
            SetSpotlightImage(WallpaperHandler.SpotlightImage);
        }

        private async void SpotlightImageReloadButton_Click(object sender, RoutedEventArgs e)
        {
            spotlightProgress.Value = spotlightProgress.Minimum;
            spotlightProgress.IsIndeterminate = true;

            SoftwareBitmap image = await GetSpotlightImage();
            WallpaperHandler.SetSpotlightImage(image);
            SetSpotlightImage(image);

            spotlightSaveToGallery.IsEnabled = true;
            WallpaperHandler.IsSpotLightImageSavedToGallery = false;
        }

        private void BingSaveToGallery_Click(object sender, RoutedEventArgs e)
        {
            if (WallpaperHandler.IsBingImageLoaded)
            {
                GalleryHandler.SaveImageToGallery(WallpaperHandler.BingImage,
                    DateTime.Now.ToString("yyyyMMdd") + ".jpg");
                WallpaperHandler.IsBingImageSavedToGallery = true;
                bingSaveToGallery.IsEnabled = !WallpaperHandler.IsBingImageSavedToGallery;
            }
        }

        private void SpotlightSaveToGallery_Click(object sender, RoutedEventArgs e)
        {
            if (WallpaperHandler.IsSpotlightImageLoaded)
            {
                GalleryHandler.SaveImageToGallery(WallpaperHandler.SpotlightImage,
                    DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg");
                WallpaperHandler.IsSpotLightImageSavedToGallery = true;
                spotlightSaveToGallery.IsEnabled = !WallpaperHandler.IsSpotLightImageSavedToGallery;
            }
        }

        // helping functions

        private async void SetBingImage(SoftwareBitmap image)
        {
            var imageSource = new SoftwareBitmapSource();
            await imageSource.SetBitmapAsync(image);
            bingImage.Source = imageSource;
            bingProgress.IsIndeterminate = false;
            bingProgress.Value = bingProgress.Maximum;
        }

        private async void SetSpotlightImage(SoftwareBitmap image)
        {
            var imageSource = new SoftwareBitmapSource();
            await imageSource.SetBitmapAsync(image);
            spotlightImage.Source = imageSource;
            spotlightProgress.IsIndeterminate = false;
            spotlightProgress.Value = spotlightProgress.Maximum;
        }

        private async Task<SoftwareBitmap> GetSoftwareBitmapSourceFromURL(string url)
        {
            var req = WebRequest.Create(url);
            var res = req.GetResponse();
            var imgStream = res.GetResponseStream();
            var memStream = new MemoryStream();
            imgStream.CopyTo(memStream);
            var decoder = await BitmapDecoder.CreateAsync(memStream.AsRandomAccessStream());
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Premultiplied);

            return softwareBitmap;
        }

        private async Task<SoftwareBitmap> GetBingImage()
        {
            await Task.Run(GetBingImageURL);
            return (await GetSoftwareBitmapSourceFromURL(bingImageURL));
        }

        private async Task<SoftwareBitmap> GetSpotlightImage()
        {
            await Task.Run(GetSpotlightImageURL);
            return (await GetSoftwareBitmapSourceFromURL(spotlightImageURL));
        }

        private void GetBingImageURL()
        {
            string xmlData;
            bingImageURL = "https://bing.com";
            using (var client = new WebClient())
                xmlData = client.DownloadString("https://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-IN");

            int[] pos = new int[2];
            pos[0] = xmlData.IndexOf("<url>") + 5;
            pos[1] = xmlData.IndexOf("</url>");

            bingImageURL += xmlData.Substring(pos[0], pos[1] - pos[0]);
        }

        private void GetSpotlightImageURL()
        {
            string jsonDataLink = "https://arc.msn.com/v3/Delivery/Placement?pid=338387&fmt=json&Lc=en-US&cdm=1&ctry=IN";
            string jsonData;
            using (var client = new WebClient())
                jsonData = client.DownloadString(jsonDataLink);

            jsonData = jsonData.Replace("\\", string.Empty);
            int[] pos = new int[2];
            pos[0] = jsonData.IndexOf("https://");

            for (int i = pos[0]; i < jsonData.Length; ++i)
            {
                if (jsonData[i] == '\"')
                {
                    pos[1] = i;
                    break;
                }
            }

            spotlightImageURL = jsonData.Substring(pos[0], pos[1] - pos[0]);
        }
    }
}

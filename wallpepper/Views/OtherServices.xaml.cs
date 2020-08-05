using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using Windows.Data.Json;
using wallpepper.Core.Helpers;

namespace wallpepper.Views
{
    public sealed partial class OtherServices : Page
    {
        private String bingImageURL, spotlightImageURL;
        public OtherServices()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage bingImg, spotlightImg;
            if (WallpaperHandler.isBingImageLoaded == false)
            {
                bingImg = await getBingImage();
                WallpaperHandler.setBingImage(bingImg);
            }
            setBingImage(WallpaperHandler.BingImage);
            if (WallpaperHandler.isSpotlightImageLoaded == false)
            {
                spotlightImg = await getSpotlightImage();
                WallpaperHandler.setSpotlightImage(spotlightImg);
            }
            setSpotlightImage(WallpaperHandler.SpotlightImage);
        }

        private async void spotlightImageReloadButton_Click(object sender, RoutedEventArgs e)
        {
            spotlightProgress.Value = spotlightProgress.Minimum;
            spotlightProgress.IsIndeterminate = true;
            BitmapImage image = await getSpotlightImage();
            WallpaperHandler.setSpotlightImage(image);
            setSpotlightImage(image);
        }

        // helping functions

        private void setBingImage(BitmapImage image)
        {
            bingImage.Source = image;
            bingProgress.IsIndeterminate = false;
            bingProgress.Value = bingProgress.Maximum;
        }

        private void setSpotlightImage(BitmapImage image)
        {
            spotlightImage.Source = image;
            spotlightProgress.IsIndeterminate = false;
            spotlightProgress.Value = spotlightProgress.Maximum;
        }

        private async Task<BitmapImage> getBingImage()
        {
            await Task.Run(getBingImageURL);
            return new BitmapImage(new Uri(bingImageURL));
        }

        private async Task<BitmapImage> getSpotlightImage()
        {
            await Task.Run(getSpotlightImageURL);
            return new BitmapImage(new Uri(spotlightImageURL));
        }

        private void getBingImageURL()
        {
            String xmlData;
            bingImageURL = "https://bing.com";
            using (var client = new WebClient())
                xmlData = client.DownloadString("https://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-IN");

            int[] pos = new int[2];
            pos[0] = xmlData.IndexOf("<url>") + 5;
            pos[1] = xmlData.IndexOf("</url>");

            bingImageURL += xmlData.Substring(pos[0], pos[1] - pos[0]);
        }

        private void getSpotlightImageURL()
        {
            DateTime time = DateTime.Now.ToUniversalTime();
            String formattedTime = time.Year.ToString() + "-"
                + time.Month.ToString() + "-"
                + time.Day.ToString() + "T"
                + time.Hour.ToString() + ":"
                + time.Minute.ToString() + ":"
                + time.Second.ToString() + "Z";

            String jsonDataLink = "https://arc.msn.com/v3/Delivery/Cache?pid=209567&fmt=json&rafb=0&ua=WindowsShellClient%2F0&disphorzres=1080&dispvertres=1920&lo=80217&pl=en-US&lc=en-US&ctry=us&time=" + formattedTime;
            String jsonData;
            using (var client = new WebClient())
                jsonData = client.DownloadString(jsonDataLink);

            jsonData = jsonData.Replace("\\", String.Empty);
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

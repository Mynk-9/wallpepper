using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace wallpepper.Views
{
    class GalleryPhotoData
    {
        public string Source { get; set; }
    }

    static class GalleryHandler
    {
        private static readonly StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        private static ObservableCollection<GalleryPhotoData> galleryPhotoCollection;
        private static bool galleryImagesLoaded = false;

        private static async Task LoadGalleryImages()
        {
            List<GalleryPhotoData> photos = new List<GalleryPhotoData>();
            var files = await storageFolder.GetFilesAsync().AsTask();
            foreach (var file in files)
            {
                string extension = file.FileType;
                if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                    continue;
                using (var filestream = await file.OpenReadAsync())
                {
                    GalleryPhotoData photo = new GalleryPhotoData
                    {
                        Source = "ms-appdata:///local/" + file.Name
                    };
                    photos.Add(photo);
                }
            }
            photos.Reverse();
            galleryPhotoCollection = new ObservableCollection<GalleryPhotoData>(photos);
            galleryImagesLoaded = true;
        }
        public static async Task<ObservableCollection<GalleryPhotoData>> GetGalleryImages()
        {
            if (!galleryImagesLoaded)
                await LoadGalleryImages();
            return galleryPhotoCollection;
        }
        public static async Task RefreshGallery()
        {
            await LoadGalleryImages();
        }

        public static async void SaveImageToGallery(SoftwareBitmap image, string name)
        {
            StorageFile file;
            try
            {
                file = await storageFolder.CreateFileAsync(name).AsTask();
            }
            catch (Exception err)
            {
                var messageBox = new ContentDialog()
                {
                    Title = "Exception at saving image to gallery",
                    Content = "Debug info:\n" + err.ToString(),
                    CloseButtonText = "Ok"
                };
                await messageBox.ShowAsync();

                return;
            }

            using (var filestream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId,
                    filestream);
                encoder.SetSoftwareBitmap(image);
                try
                {
                    await encoder.FlushAsync();

                    galleryPhotoCollection.Add(new GalleryPhotoData()
                    {
                        Source = "ms-appdata:///local/" + file.Name
                    });
                }
                catch (Exception err)
                {
                    var messageBox = new ContentDialog()
                    {
                        Title = "Exception at saving image to gallery",
                        Content = "Debug info:\n" + err.ToString(),
                        CloseButtonText = "Ok"
                    };
                    await messageBox.ShowAsync();
                }
            }
        }
    }
}

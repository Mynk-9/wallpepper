using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Popups;

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
            galleryPhotoCollection = new ObservableCollection<GalleryPhotoData>(photos);
            galleryImagesLoaded = true;
        }
        public static async Task<ObservableCollection<GalleryPhotoData>> GetGalleryImages()
        {
            Debug.WriteLine(storageFolder.Path);
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
            StorageFile file = await storageFolder.CreateFileAsync(name).AsTask();
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
                    var messageDialog = new MessageDialog("Exception at saving image to gallery! " +
                        "Debug information:\n" +
                        err.ToString());

                    await messageDialog.ShowAsync();
                }
            }
        }
    }
}

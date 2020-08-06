using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

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
    }
}

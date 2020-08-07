using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace wallpepper.Views
{
    public class ImageScaleConverter16 : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, string language) => 16 * (double)value;
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ImageScaleConverter9 : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, string language) => 9 * (double)value;
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public sealed partial class Gallery : Page, INotifyPropertyChanged
    {
        public Gallery() => InitializeComponent();

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var thisAdaptiveGrid = ((Page)sender).FindDescendantByName("AdaptiveGridViewControl") as AdaptiveGridView;
            thisAdaptiveGrid.ItemsSource = await GalleryHandler.GetGalleryImages();
            thisAdaptiveGrid.ItemClick += ThisAdaptiveGrid_ItemClick;
        }

        private async void ThisAdaptiveGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as GalleryPhotoData;
            var source = item.Source;

            var messageBox = new ContentDialog()
            {
                Title = "Desktop BAckground",
                Content = "Do you want to make the image as desktop background?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };
            var res = await messageBox.ShowAsync();

            if (res == ContentDialogResult.Primary)
                MakeDesktopBackground(source);
        }

        private void MakeDesktopBackground(string path)
        {

        }
    }
}

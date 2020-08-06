using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace wallpepper.Views
{
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
        }
    }
}

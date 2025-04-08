using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using SerwisFilmowy.Repositories;
using SerwisFilmowy.Model;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;

namespace SerwisFilmowy
{

    public sealed partial class Create : Page
    {
        private readonly IMovieRepository _movieRepository = new MovieRepository();

        private byte[] _selectedImageBytes;

        private Main _main;

        private bool canSave = true;
        private bool isEmpty = true;

        public Create()
        {
            this.InitializeComponent();

        }

        private void Back_Click(object sender, RoutedEventArgs e) {
            ContentFrame.Navigate(typeof(Main), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }



        private void Save_Click(object sender, RoutedEventArgs e) {

            IsEmpty();

            if (canSave && !isEmpty) {
                Movies movie = new Movies() { Title = TitleBox.Text, Genre = GenereBox.Text, Year = int.Parse(YearBox.Text), Director = DirectorBox.Text, Staff = CastBox.Text, Description = DescriptionBox.Text, Image = _selectedImageBytes };

                _movieRepository.Create(movie);

                _main.readList();

                ContentFrame.Navigate(typeof(Main), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }

        }

        private void IsEmpty() {
            if (TitleBox.Text == "" || GenereBox.Text == "" || YearBox.Text == "" || DirectorBox.Text == "" || CastBox.Text == "" || DescriptionBox.Text == "" || _selectedImageBytes == null) {
                isEmpty = true;
            }
            else {
                isEmpty = false;
            }
        }


        private void TitleLostFocus(object sender, RoutedEventArgs e) {
            if(_main.moviesListTitle.Contains(TitleBox.Text)) {
                TitleBox.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100,255,0,0));
                TitleBox.BorderThickness = new Thickness(2);
                canSave = false;
            }
            else {
                TitleBox.BorderThickness = new Thickness(0);
                canSave = true;
            }
        }


        private void YearTxtPreviewKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e) {
            if (!char.IsDigit((char)e.Key) && e.Key != Windows.System.VirtualKey.Back && e.Key != Windows.System.VirtualKey.Tab && 
                e.Key != Windows.System.VirtualKey.Home && e.Key != Windows.System.VirtualKey.End && 
                e.Key != Windows.System.VirtualKey.Control && e.Key != Windows.System.VirtualKey.Left && e.Key != Windows.System.VirtualKey.Right) {

                e.Handled = true;
            }
        }

        
        private void YearTxtLostFocus(object sender, RoutedEventArgs e) {
            if (int.Parse(YearBox.Text) > 2030 || int.Parse(YearBox.Text) < 1888) {
                YearBox.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 255, 0, 0));
                YearBox.BorderThickness = new Thickness(2);
                canSave = false;
            }
            else {
                YearBox.BorderThickness = new Thickness(0);
                canSave = true;
            }
        }




        private async void LoadImage_Click(object sender, RoutedEventArgs e) {
            //disable the button to avoid double-clicking
            var senderButton = sender as Button;
            senderButton.IsEnabled = false;

            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = (Application.Current as App)?.m_window;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null) {
                _selectedImageBytes = File.ReadAllBytes(file.Path);

                noChooseTxt.Visibility = Visibility.Collapsed;
                poster.Source = new BitmapImage(new Uri(file.Path));
            }

            //re-enable the button
            senderButton.IsEnabled = true;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter is Main main) {
                _main = main;
            }
        }
    }
}

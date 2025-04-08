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
using Microsoft.UI.Xaml.Media.Animation;
using SerwisFilmowy.Model;
using Windows.Storage.Pickers;
using SerwisFilmowy.Repositories;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Reflection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SerwisFilmowy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Update : Page
    {
        private readonly IMovieRepository _movieRepository = new MovieRepository();

        private byte[] _selectedImageBytes;

        private Main _main;

        public Update()
        {
            this.InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e) {
            ContentFrame.Navigate(typeof(Main), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
        }



        private void Save_Click(object sender, RoutedEventArgs e) {
            Movies movie = new Movies() { Id = 2, Title = TitleBox.Text, Genre = GenereBox.Text, Year = int.Parse(YearBox.Text), Director = DirectorBox.Text, Staff = CastBox.Text, Description = DescriptionBox.Text, Image = _selectedImageBytes };

            _movieRepository.Update(movie);
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
            }

            //re-enable the button
            senderButton.IsEnabled = true;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter is Main main) {
                _main = main;
            }

            Movies movie = _movieRepository.Read(_main.selectedTitle);

            TitleBox.Text = movie.Title;
            GenereBox.Text = movie.Genre;
            YearBox.Text = movie.Year.ToString();
            DirectorBox.Text = movie.Director;
            CastBox.Text = movie.Staff;
            DescriptionBox.Text = movie.Description;

        }

    }
}

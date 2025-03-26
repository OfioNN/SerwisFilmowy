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

namespace SerwisFilmowy
{

    public sealed partial class Create : Page
    {
        private readonly IMovieRepository _movieRepository = new MovieRepository();

        private byte[] _selectedImageBytes;

        public Create()
        {
            this.InitializeComponent();

        }

        #region Close Ellipse
        private void Click_Close(object sender, RoutedEventArgs e) {
            var window = (Application.Current as App)?.m_window;
            window?.Close();
        }

        private void Entered_Close(object sender, RoutedEventArgs e) {
            CloseEllipse.Fill = new SolidColorBrush(Colors.Firebrick);
            StackPanelHand.changeCursor();
        }

        private void Exited_Close(object sender, RoutedEventArgs e) {
            CloseEllipse.Fill = new SolidColorBrush(Colors.Red);
        }
        #endregion

        #region Minimize Ellipse
        private void Click_Minimize(object sender, RoutedEventArgs e) {
            var window = (Application.Current as App)?.m_window;

            if (window.AppWindow.Presenter is OverlappedPresenter presenter) {
                presenter.Minimize();
            }
        }

        private void Entered_Minimize(object sender, RoutedEventArgs e) {
            MinimizeEllipse.Fill = new SolidColorBrush(Colors.Gold);
            StackPanelHand.changeCursor();
        }

        private void Exited_Minimize(object sender, RoutedEventArgs e) {
            MinimizeEllipse.Fill = new SolidColorBrush(Colors.Yellow);
        }
        #endregion

        private async void Save_Click(object sender, RoutedEventArgs e) {
            Movies movie = new Movies() { Title = TitleBox.Text, Director = DirectorBox.Text, Genre = YearBox.Text, Description = DescriptionBox.Text, Image = _selectedImageBytes };

            _movieRepository.Create(movie);
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
            else {

            }

            //re-enable the button
            senderButton.IsEnabled = true;

        }
    }
}

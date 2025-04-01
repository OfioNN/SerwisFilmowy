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
using Windows.Graphics;
using Microsoft.UI.Windowing;
using Windows.UI.Core;
using Microsoft.UI;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Input;
using System.Security.Cryptography.X509Certificates;
using System.Net.Sockets;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using System.Runtime.InteropServices;
using SerwisFilmowy.Model;
using SerwisFilmowy.Repositories;
using Microsoft.UI.Xaml.Media.Animation;

namespace SerwisFilmowy
{

    public sealed partial class MainWindow : Window
    {

        private List<string> Movies = new List<string>()
        {
            "Oppenheimer",
            "Good Will Hunting",
            "The Godfather",
            "The Shawshank Redemption",
            "The Dark Knight",
            "Forrest Gump",
            "The Matrix",
            "The Lord of the Rings",
            "Fight Club",
            "Inception"
        };



        public MainWindow()
        {
            this.InitializeComponent();

            //(960, 540) V (1280, 720)
            this.AppWindow.Resize(new SizeInt32(1280, 720));

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(gridMove);

            if (AppWindow.Presenter is OverlappedPresenter presenter) {
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = false;
                presenter.SetBorderAndTitleBar(true, false);
            }
        }


        #region Close Ellipse
        private void Click_Close(object sender, RoutedEventArgs e) {
            this.Close();

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

            if (this.AppWindow.Presenter is OverlappedPresenter presenter) {
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

        private void Dodaj_Click(object sender, RoutedEventArgs e) {

            ContentFrame.Navigate(typeof(Create), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }


        // Handle text change and present suitable items
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var movie in Movies) {
                    var found = splitText.All((key) =>
                    {
                        return movie.ToLower().Contains(key);
                    });
                    if (found) {
                        suitableItems.Add(movie);
                    }
                }
                if (suitableItems.Count == 0) {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }
        }

        // Handle user selecting an item, in our case just output the selected item.
        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
            SuggestionOutput.Text = args.SelectedItem.ToString();
        }

    }



    public class StackPanelControls : StackPanel {
        public void changeCursor() {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }
    }
}

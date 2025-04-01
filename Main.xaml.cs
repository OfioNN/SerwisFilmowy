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
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI;
using SerwisFilmowy.Model;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SerwisFilmowy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Main : Page
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


        public Main()
        {
            this.InitializeComponent();
        }


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
                    var found = splitText.All((key) => {
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
}


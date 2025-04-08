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
using SerwisFilmowy.Repositories;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.Graphics.Printing;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SerwisFilmowy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Main : Page {
        private readonly IMovieRepository _movieRepository = new MovieRepository();

        private List<string> moviesListTitle = new List<string>();
        List<Movies> moviesList = new List<Movies>();
        public string selectedTitle;

        public Main() {
            this.InitializeComponent();

            readList();

        }

        public void readList() {
            moviesList.Clear();
            listView.Items.Clear();

            moviesList = _movieRepository.ReadAll();
            moviesList = moviesList.OrderBy(x => x.Title).ToList();

            foreach (var movie in moviesList) {
                listView.Items.Add(movie.Title);
                moviesListTitle.Add(movie.Title);
            }
        }


        private void Dodaj_Click(object sender, RoutedEventArgs e) {

            ContentFrame.Navigate(typeof(Create), this, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private void Usun_Click(object sender, RoutedEventArgs e) {
            Movies movie = new Movies() { Title = selectedTitle };

            _movieRepository.Delete(movie);
            usunBtn.IsEnabled = false;

            readList();
        }

        private void Edytuj_Click(object sender, RoutedEventArgs e) {
            ContentFrame.Navigate(typeof(Update), this, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });


        }

        private async void listView_ItemClick(object sender, ItemClickEventArgs e) {

            selectedTitle = (string)e.ClickedItem;

            Movies movie = _movieRepository.Read(selectedTitle);

            titleTxt.Text = movie.Title;
            genereTxt.Text = movie.Genre;
            yearTxt.Text = movie.Year.ToString();
            directorTxt.Text = movie.Director;
            castTxt.Text = movie.Staff;
            descriptionTxt.Text = movie.Description;

            if (movie.Image != null && movie.Image.Length > 0) {
                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(movie.Image)) {
                    await bitmapImage.SetSourceAsync(ms.AsRandomAccessStream());
                }
                poster.Source = bitmapImage;
            }

            usunBtn.IsEnabled = true;
        }


        


        // Handle text change and present suitable items
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var movie in moviesListTitle) {
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


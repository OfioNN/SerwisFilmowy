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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SerwisFilmowy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Create : Page
    {
        public Create()
        {
            this.InitializeComponent();
        }

        #region Close Ellipse
        private void Click_Close(object sender, RoutedEventArgs e) {
            Window.Current.Close();

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

            if (Window.Current.AppWindow.Presenter is OverlappedPresenter presenter) {
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
    }
}

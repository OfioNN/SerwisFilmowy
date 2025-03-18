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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SerwisFilmowy
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 


    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            this.InitializeComponent();

            this.AppWindow.Resize(new SizeInt32(960, 540));

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(mainGrid);

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

    }



    public class StackPanelControls : StackPanel {
        public void changeCursor() {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }
    }
}

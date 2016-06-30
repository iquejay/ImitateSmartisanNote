using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImitateSmartisanNote
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.ui_imageTitle.MouseLeftButtonDown += Ui_imageTitle_MouseLeftButtonDown;
            App.MainFrame = MainFrame;
            this.MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            this.MainFrame.Navigate(new Uri("MainPage.xaml",UriKind.Relative));
        }

        private void Ui_imageTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseTb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}

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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Construct_Main
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
             
            InitializeComponent();
            MainFrame.Navigate(new View.OrderPage());
            SideMenuBar.GetButton("Catalog").Click += MainWindow_Click;
            SideMenuBar.GetButton("Busket").Click += MainWindow_Click1;
            SideMenuBar.GetButton("Orders").Click += MainWindow_Click2;
        }

        private void MainWindow_Click2(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new View.OrderPage());
            SideMenuBar.CloseSide();
        }

        private void MainWindow_Click1(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new View.BusketPage());
            SideMenuBar.CloseSide();
        }

        private void MainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new View.CatalogPage());
            SideMenuBar.CloseSide();
        }

        private void DragRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation da = new ThicknessAnimation();
            da.Duration = TimeSpan.FromSeconds(0.3);
            da.EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut };
            da.To = new System.Windows.Thickness(0,30,0,0);
            SideMenuBar.BeginAnimation(MarginProperty, da);

        }
    }
}

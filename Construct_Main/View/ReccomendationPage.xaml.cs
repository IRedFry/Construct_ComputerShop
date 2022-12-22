using BLL;
using Construct_Main.ViewModel;
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

namespace Construct_Main.View
{
    /// <summary>
    /// Логика взаимодействия для ReccomendationPage.xaml
    /// </summary>
    public partial class ReccomendationPage : Page
    {
        private RecommendationViewModel VM;
        public ReccomendationPage(List<ProductModel> products, IRecommendationService recommendationService, IAutorizationService autorizationService, MainViewModel mainWindow, OrderModel busket)
        {
            InitializeComponent();
            VM = new RecommendationViewModel(products, recommendationService, autorizationService, mainWindow, busket);
            DataContext = VM;
        }
    }
}

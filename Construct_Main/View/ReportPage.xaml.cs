using BLL;
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
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        private ViewModel.ReportViewModel VM;
        public ReportPage(IReportService reportService, IAutorizationService autorizationService, IForecastService forecastService)
        {
            VM = new ViewModel.ReportViewModel(reportService, autorizationService, forecastService);
            DataContext = VM;
            InitializeComponent();
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            VM.RecalculateData(From.SelectedDate, To.SelectedDate);
        }

    }
}

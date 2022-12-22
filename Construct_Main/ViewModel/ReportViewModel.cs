using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construct_Main.ViewModel
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private IReportService reportService;
        private IAutorizationService autorizationService;
        private IForecastService forecastService;

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _countOrders = 0;
        public int CountOrders
        {
            get { return _countOrders; }
            set
            {
                _countOrders = value;
                NotifyPropertyChanged("CountOrders");
            }
        }

        private int _counComplete = 0;
        public int CountComplete
        {
            get { return _counComplete; }
            set
            {
                _counComplete = value;
                NotifyPropertyChanged("CountComplete");
            }
        }

        private int _countCancel = 0;
        public int CountCancel
        {
            get { return _countCancel; }
            set
            {
                _countCancel = value;
                NotifyPropertyChanged("CountCancel");
            }
        }

        private int _countOrdered = 0;
        public int CountOrdered
        {
            get { return _countOrdered; }
            set
            {
                _countOrdered = value;
                NotifyPropertyChanged("CountOrdered");
            }
        }

        private int _countVerified = 0;
        public int CountVerified
        {
            get { return _countVerified; }
            set
            {
                _countVerified = value;
                NotifyPropertyChanged("CountVerified");
            }
        }

        private decimal _clearProfit = 0;
        public decimal ClearProfit
        {
            get { return _clearProfit; }
            set
            {
                _clearProfit = value;
                NotifyPropertyChanged("ClearProfit");
            }
        }

        private bool _isft = true;
        public bool IsFromTo
        {
            get { return _isft; }
            set
            {
                _isft = value;
                NotifyPropertyChanged("IsFromTo");
            }
        }

        private decimal _forecastwage = 0;
        public decimal ForecastWage
        {
            get { return _forecastwage; }
            set
            {
                _forecastwage = value;
                NotifyPropertyChanged("ForecastWage");
            }
        }

        private decimal _forecastaward = 0;
        public decimal ForecastAward
        {
            get { return _forecastaward; }
            set
            {
                _forecastaward = value;
                NotifyPropertyChanged("ForecastAward");
            }
        }
        #endregion

        #region Commands
        private RelayCommand changereport;
        public RelayCommand ChangeReportCommand
        {
            get
            {
                return changereport ?? (changereport = new RelayCommand(obj =>
                {
                    ChangeReport(Boolean.Parse((string)obj));
                }));
            }
        }
        #endregion

        public ReportViewModel(IReportService reportService, IAutorizationService autorizationService, IForecastService forecastService)
        {
            this.reportService = reportService;
            this.autorizationService = autorizationService;
            this.forecastService = forecastService;

            Forecast f = forecastService.ForecastForSeller(autorizationService.GetCurrentUser().id);

            ForecastWage = f.ForecastWage;
            ForecastAward = f.ForecastAward;
        }

        public void RecalculateData(DateTime? from, DateTime? to)
        {
            if (from != null && to != null && to > from)
            {
                var AllOrders = reportService.ExecuteSP((DateTime)from, (DateTime)to).Where(i => i.Seller == autorizationService.GetCurrentUser().id).ToList();
                ClearProfit = AllOrders.Where(i => i.Status == 0).Sum(i => i.TotalCost);
                CountComplete = AllOrders.Where(i => i.Status == 0).Count();
                CountCancel = AllOrders.Where(i => i.Status == 4).Count();
                CountOrdered = AllOrders.Where(i => i.Status == 1).Count();
                CountVerified = AllOrders.Where(i => i.Status == 2).Count();
                CountOrders = AllOrders.Count();
            }
            else
            {
                ClearProfit = 0;
                CountComplete = 0;
                CountCancel = 0;
                CountOrdered = 0;
                CountVerified = 0;
                CountOrders = 0;
            }
        }

        public void ChangeReport(bool value)
        {
            IsFromTo = value;
        }
    }
}

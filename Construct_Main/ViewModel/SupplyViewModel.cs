using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construct_Main.ViewModel
{
    public class SupplyViewModel : INotifyPropertyChanged
    {
        public struct AddSupplyLine
        {
            public int Id { get; set; }
            public bool ChoiceProduct { get; set; }
            public bool NewProduct { get; set; }
            public bool ExistProduct { get; set; }
        }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private decimal _priceText = 0;
        public string PriceText
        {
            get { return _priceText.ToString() + " Руб."; }
            set
            {
                _priceText = Decimal.Parse(value);
                NotifyPropertyChanged("PriceText");
            }
        }
        #endregion

        #region Commands

        private RelayCommand addSupplyProduct;
        public RelayCommand AddSupplyProductCommand
        {
            get
            {
                return addSupplyProduct ?? (addSupplyProduct = new RelayCommand(obj =>
                {
                    AddSupplyProduct();
                }));
            }
        }

        private RelayCommand changesuplytype;
        public RelayCommand ChangeSupplyTypeCommand
        {
            get
            {
                return changesuplytype ?? (changesuplytype = new RelayCommand(obj =>
                {
                    var values = (object[])obj;
                    string realV = (string)values[1];
                    int pId = AddProductLines[Int32.Parse((string)values[0])].Id;
                    switch (realV)
                    {
                        case "Новый товар":
                            {
                                AddProductLines[pId] = new AddSupplyLine { Id = pId, ChoiceProduct = false, ExistProduct = false, NewProduct = true };
                            }
                            break;
                        case "Существующий товар":
                            {
                                AddProductLines[pId] = new AddSupplyLine { Id = pId, ChoiceProduct = false, ExistProduct = true, NewProduct = false };
                            }
                            break;
                    }

                    NotifyPropertyChanged("AddProductLines");
                }));
            }
        }

        #endregion

        public ObservableCollection<AddSupplyLine> AddProductLines { get; set; }
        public SupplyViewModel()
        {
            AddProductLines = new ObservableCollection<AddSupplyLine>();
        }

        private void AddSupplyProduct()
        {
            int id = AddProductLines.Count == 0 ? 0 : AddProductLines.OrderByDescending(i => i.Id).FirstOrDefault().Id + 1;
            AddProductLines.Add(new AddSupplyLine {Id =  id, ChoiceProduct = true, ExistProduct = false, NewProduct = false });
        }
    }
}

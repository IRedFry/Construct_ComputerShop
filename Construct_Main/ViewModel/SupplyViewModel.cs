using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using BLL;

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

        public struct SupplyLine
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Count { get; set; }
            public string CountString { get; set; }
            public Uri CategoryImageSource { get; set; }
            public decimal Price { get; set; }
            public string PriceString { get; set; }
        }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private decimal _priceText = 0;
        public decimal PriceText
        {
            get { return _priceText; }
            set
            {
                _priceText = value;
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


        private RelayCommand addnewproduct;
        public RelayCommand AddNewProductCommand
        {
            get
            {
                return addnewproduct ?? (addnewproduct = new RelayCommand(obj =>
                { 
                    var values = (object[])obj;
                    int line_id = Int32.Parse((string)values[0]);
                    if (values[1] == null || values[2] == null || values[3] == null || values[4] == null || values[5] == null || values[6] == null)
                        return;

                    string name = (string)values[1];
                    int cat_id = (int)values[2];
                    int manuf_id = (int)values[3];
                    decimal price = (decimal)values[4];
                    string description = (string)values[5];
                    decimal count = (decimal)values[6];
                    decimal supply_price = (decimal)values[7];

                    AddNewProduct(line_id, name, cat_id, manuf_id, price, description, (int)count, supply_price);
                }));
            }
        }


        private RelayCommand addexistproduct;
        public RelayCommand AddExistProductCommand
        {
            get
            {
                return addexistproduct ?? (addexistproduct = new RelayCommand(obj =>
                {
                    var values = (object[])obj;
                    int line_id = Int32.Parse((string)values[0]);
                    if (values[1] == null || values[2] == null)
                        return;

                    int product_id = (int)values[1];
                    decimal count = (decimal)values[2];
                    decimal supply_price = (decimal)values[3];
                    AddExistProduct(line_id, product_id, (int)count, supply_price);
                }));
            }
        }

        private RelayCommand removeproduct;
        public RelayCommand RemoveProductCommand
        {
            get
            {
                return removeproduct ?? (removeproduct = new RelayCommand(obj =>
                {
                    RemoveProduct((int)obj);
                }));
            }
        }

        private RelayCommand makesupply;
        public RelayCommand MakeSupplyCommand
        {
            get
            {
                return makesupply ?? (makesupply = new RelayCommand(obj =>
                {
                    MakeSupply((int)obj);
                }));
            }
        }
        #endregion

        private IDbCrud context;
        private ISupplyService supplyService;
        public ObservableCollection<AddSupplyLine> AddProductLines { get; set; }
        public ObservableCollection<SupplyLine> SupplyLines { get; set; }
        public List<SupplierModel> Suppliers { get; set; }
        public List<ProductModel> Products { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<ManufacturerModel> Manufacturers { get; set; }
        public SupplyViewModel(IDbCrud db, ISupplyService supplyService, List<ProductModel> pr)
        {
            AddProductLines = new ObservableCollection<AddSupplyLine>();
            SupplyLines = new ObservableCollection<SupplyLine>();
            this.context = db;
            Suppliers = context.GetAllSuppliers();
            Products = pr;
            Categories = context.GetAllCategories();
            Manufacturers = context.GetAllManufacturers();
            this.supplyService = supplyService;
        }

        private void AddSupplyProduct()
        {
            int id = AddProductLines.Count == 0 ? 0 : AddProductLines.OrderByDescending(i => i.Id).FirstOrDefault().Id + 1;
            AddProductLines.Add(new AddSupplyLine {Id =  id, ChoiceProduct = true, ExistProduct = false, NewProduct = false });
        }

        private void AddNewProduct(int line_id, string name, int id_cat, int id_manuf, decimal price, string description, int count, decimal supply_price)
        {
            AddProductLines.RemoveAt(line_id);

            var p = new ProductModel
            {
                Id = -1,
                Description = description,
                Price = price,
                CategoryId = id_cat,
                ManufId = id_manuf,
                Count = 0,
                Name = name,
                Sale = 0
            };
            int id = context.CreateProduct(p);
            p = context.GetProduct(id);
            
            SupplyLines.Add(new SupplyLine 
            {
                ProductId = id,
                ProductName = p.Name,
                CountString = count.ToString() + " шт.",
                Count = count,
                CategoryImageSource = p.CategoryImageSource,
                Price = supply_price,
                PriceString = supply_price.ToString() + " руб"
            });


            PriceText = PriceText + supply_price * count;
        }

        private void AddExistProduct(int line_id, int product_id, int count, decimal supply_price)
        {
            AddProductLines.RemoveAt(line_id);
            var p = context.GetProduct(product_id);

            var sl = SupplyLines.Where(i => i.ProductId == product_id).FirstOrDefault();

            if (sl.ProductName == null)
            {
                SupplyLines.Add(new SupplyLine
                {
                    ProductId = product_id,
                    ProductName = p.Name,
                    Count = count,
                    CountString = count.ToString() + " шт.",
                    CategoryImageSource = p.CategoryImageSource,
                    Price = supply_price,
                    PriceString = supply_price.ToString() + " руб"
                });
            }
            else
            {
                SupplyLines.Remove(sl);
                SupplyLines.Add(new SupplyLine
                {
                    ProductId = sl.ProductId,
                    CategoryImageSource = sl.CategoryImageSource,
                    Count = sl.Count + count,
                    ProductName = sl.ProductName,
                    Price = supply_price,
                    CountString = (sl.Count + count).ToString() + " шт.",
                    PriceString = supply_price.ToString() + " руб"
                });
            }

            PriceText = PriceText + supply_price * count;
        }

        private void RemoveProduct(int id)
        {
            var p = SupplyLines.Where(i => i.ProductId == id).FirstOrDefault();
            SupplyLines.Remove(p);
            PriceText = PriceText - p.Price * p.Count;
        }

        private void MakeSupply(int sup_id)
        {
            SupplyModel sup = new SupplyModel
            {
                Id = -1,
                Date = DateTime.Now,
                ProductCounts = SupplyLines.Select(i => i.Count).ToList(),
                ProductPrices = SupplyLines.Select(i => i.Price).ToList(),
                ProductsIds = SupplyLines.Select(i => i.ProductId).ToList(),
                SupplierId = sup_id,
                TotalCost = SupplyLines.Select(i => i.Count * i.Price).Sum()
            };
            supplyService.MakeSupply(sup);

            SupplyLines.Clear();
            AddProductLines.Clear();
            PriceText = 0;

            
        }
    }
}

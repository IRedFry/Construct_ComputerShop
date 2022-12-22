using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BLL;

namespace Construct_Main.ViewModel
{
    public class RecommendationViewModel
    {
        private List<ProductModel> products;
        private IRecommendationService recommendationService;
        private IAutorizationService autorizationService;
        private MainViewModel mainWindow;
        private OrderModel Busket;

        private int curType = 0;

        #region Commands

        private RelayCommand addtobusket;

        public RelayCommand AddToBusketCommand
        {
            get
            {
                return addtobusket ?? (addtobusket = new RelayCommand(obj =>
                {
                    AddProductToBusket(Int32.Parse((string)obj));
                }));
            }
        }

        private RelayCommand removeProduct;
        public RelayCommand RemoveProductCommand
        {
            get
            {
                return removeProduct ?? (removeProduct = new RelayCommand(obj =>
                {
                    RemoveProduct(Int32.Parse((string)obj));
                }));
            }
        }

        private RelayCommand changeReccomendation;
        public RelayCommand ChangeReccomendationCommand
        {
            get
            {
                return changeReccomendation ?? (changeReccomendation = new RelayCommand(obj =>
                {
                    var type = (string)(obj);
                    switch(type)
                    {
                        case "Popular":
                            SetPopularProducts();
                            break;

                        case "Category":
                            SetCategoryProducts();
                            break;

                        case "Last":
                            SetLastProducts();
                            break;
                    }
                }));
            }
        }
        private RelayCommand toproductpage;
        public RelayCommand ToProductPageCommand
        {
            get
            {
                return toproductpage ?? (toproductpage = new RelayCommand(obj =>
                {
                    ToProductPage(Int32.Parse((string)obj));
                }));
            }
        }

        #endregion

        public ObservableCollection<ProductModel> Products { get; set; }
        public RecommendationViewModel(List<ProductModel> products, IRecommendationService recommendationService, IAutorizationService autorizationService, MainViewModel mainWindow, OrderModel busket)
        {
            this.products = products;
            this.recommendationService = recommendationService;
            this.autorizationService = autorizationService;

            Products = new ObservableCollection<ProductModel>();

            SetPopularProducts();
            this.mainWindow = mainWindow;
            Busket = busket;
        }

        private void UpdateProducts()
        {
            switch (curType)
            {
                case 1:
                    SetPopularProducts();
                    break;
                case 2:
                    SetCategoryProducts();
                    break;
                case 3:
                    SetLastProducts();
                    break;
            }

        }
        private void SetProducts(List<ProductModel> pr)
        {
            Products.Clear();
            if (pr != null)
                foreach (var item in pr)
                {
                    if (products.Where(i => i.Id == item.Id).FirstOrDefault().IsInBusket)
                        item.IsInBusket = true;
                    Products.Add(item);
                }
        }
        public void AddProductToBusket(int id)
        {
            //OrderModel or = db.GetAllOrders().Where(i => i.CustomerId == 0 && i.Status == 3).FirstOrDefault();
            //if (or != null)
            //{
            //    OrderLineModel o = new OrderLineModel
            //    {
            //        Count = 1,
            //        Id = -1,
            //        ProductId = id,
            //        OrderId = or.Id
            //    };

            //    db.AddOrderLine(o);
            //}
            //else
            //{
            //    int Oid = db.CreateBusket();
            //    OrderLineModel o = new OrderLineModel
            //    {
            //        Count = 1,
            //        Id = -1,
            //        ProductId = id,
            //        OrderId = Oid
            //    };

            //    db.AddOrderLine(o);
            //}
            
            Busket.ProductsIds.Add(id);
            Busket.ProductCounts.Add(1);
            Busket.TotalCost += products.Where(i => i.Id == id).FirstOrDefault().Price;

            products.Where(i => i.Id == id).FirstOrDefault().IsInBusket = true;
            UpdateProducts();
        }
        public void RemoveProduct(int id)
        {
            for (int i = 0; i < Busket.ProductsIds.Count; i++)
            {
                if (Busket.ProductsIds[i] == id)
                {
                    Busket.ProductsIds.RemoveAt(i);
                    Busket.ProductCounts.RemoveAt(i);
                    Busket.TotalCost -= products.Where(a => a.Id == id).FirstOrDefault().Price;
                }
            }

            products.Where(i => i.Id == id).FirstOrDefault().IsInBusket = false;
            UpdateProducts();
        }

        public void SetPopularProducts()
        {
            curType = 1;
            SetProducts(recommendationService.GetPopularProducts());
        }

        public void SetCategoryProducts()
        {
            curType = 2;
            SetProducts(recommendationService.GetRecommenProductsBasedOnCategory(autorizationService.GetCurrentUser().id));
        }

        public void SetLastProducts()
        {
            curType = 3;
            SetProducts(recommendationService.GetLastProducts());
        }

        public void ToProductPage(int id)
        {
            mainWindow.ToProductPage(id);
        }
    }
}

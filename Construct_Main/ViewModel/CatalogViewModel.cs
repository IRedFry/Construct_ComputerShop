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
    public class CatalogViewModel
    {
        private MainViewModel mainWindow;
        private OrderModel cart;

        private string searchRequest;
        private decimal lowPrice;
        private decimal topPrice;
        private int categoryId;
        private bool hasSale;
        private bool noSale;

        #region Commands

        private RelayCommand applyfiltercommand;
        private RelayCommand addtobusket;
        private RelayCommand removeProduct;
        private RelayCommand toproductpage;
        public RelayCommand ApplyFilterCommand 
        { 
            get { return applyfiltercommand ?? (applyfiltercommand = new RelayCommand(obj => 
            {
                var values = (object[])obj;
                decimal low =  (decimal)values[0];
                decimal top = (decimal)values[1];
                int catid = values[2] == null ? -1 : (int)values[2];
                bool hasS = (bool)values[3];
                bool noS = (bool)values[4];
                SetFilters(low, top, catid, hasS, noS);
            }));
            }
        }

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
        #endregion

        public ObservableCollection<ProductModel> Products { get; set; }
        private List<ProductModel> productModels { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public CatalogViewModel(List<ProductModel> productModels, List<CategoryModel> categories, MainViewModel mainWindow, OrderModel cart)
        {
            Products = new ObservableCollection<ProductModel>();

            this.productModels = productModels;

            SetProducts(productModels);
            Categories = categories;
            this.mainWindow = mainWindow;
            this.cart = cart;
        }

        private void SetProducts(List<ProductModel> pr)
        {
            Products.Clear();
            foreach (var item in pr)
                Products.Add(item);
        }
        public void SetFilters(decimal low, decimal top, int catId, bool hasS, bool noS)
        {
            lowPrice = low;
            topPrice = top;
            categoryId = catId;
            hasSale = hasS;
            noSale = noS;
            ApplyFilter(searchRequest);
        }
        public void ApplyFilter(string request)
        {
            request = request ?? "";
            foreach (char c in request)
                if (c == ' ')
                    request = request.Remove(0, 1);
                else
                    break;
            searchRequest = request;

            List<ProductModel> pr;

            if (request.Length != 0)
                pr = productModels.Where(i => i.Name.ToLower().StartsWith(request.ToLower())).ToList();
            else
                pr = productModels;

            if (lowPrice > 0)
                pr = pr.Where(i =>i.Price > lowPrice).ToList();
            if (topPrice > lowPrice)
                pr = pr.Where(i => i.Price < topPrice).ToList();
            if (categoryId != -1)
                pr = pr.Where(i => i.CategoryId == categoryId).ToList();
            if (!(hasSale && noSale))
                if (hasSale)
                    pr = pr.Where(i => i.Sale > 0).ToList();
                else
                    pr = pr.Where(i => i.Sale == 0).ToList();

            SetProducts(pr);

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

            cart.ProductsIds.Add(id);
            cart.ProductCounts.Add(1);
            cart.TotalCost += productModels.Where(i => i.Id == id).FirstOrDefault().Price;

            productModels.Where(i => i.Id == id).FirstOrDefault().IsInBusket = true;

            ApplyFilter(searchRequest);
        }
        public void RemoveProduct(int id)
        {
            //OrderModel or = db.GetAllOrders().Where(i => i.CustomerId == 0 && i.Status == 3).FirstOrDefault();
            //db.DeleteOrderLine(or.Id, id);

            for (int i = 0; i < cart.ProductsIds.Count; i++)
            {
                if (cart.ProductsIds[i] == id)
                {
                    cart.ProductsIds.RemoveAt(i);
                    cart.ProductCounts.RemoveAt(i);
                    cart.TotalCost -= productModels.Where(a => a.Id == id).FirstOrDefault().Price;
                }
            }

            productModels.Where(i => i.Id == id).FirstOrDefault().IsInBusket = false;

            ApplyFilter(searchRequest);
        }

        public void ToProductPage(int id)
        {
            mainWindow.ToProductPage(id);
        }
    }
}

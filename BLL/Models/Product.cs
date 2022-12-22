using DAL;
using System;
using System.IO;
using System.Linq;

// ------------------------------------------------------------------------------
namespace BLL
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PriceString { get; set; }
        public int Sale { get; set; }
        public int ManufId { get; set; }
        public int CategoryId { get; set; }
        public Uri CategoryImageSource { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public bool IsInBusket { get; set; }
        public ProductModel() { }

        public ProductModel(Product p)
        {
            Id = p.id;
            Name = p.name;
            Price = (int)p.price;
            PriceString = Price.ToString() + " Р.";
            Sale = (int)p.sale;
            ManufId = (int)p.id_manufacturer;
            CategoryId = (int)p.id_category;
            string v = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Sources\\Category_" + CategoryId.ToString() + ".png";
            CategoryImageSource = new Uri(v);
            Description = p.description;
            Count = (int)p.count;
            IsInBusket = p.Order_line.Where(i => i.OrderC.id_status == 3).FirstOrDefault() == null ? false : true; 
            
        }
    }
}

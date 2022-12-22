using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class ReportRepositorySQL : IReportsRepositoty
    {
        private ComputerShopContext context;

        public ReportRepositorySQL(ComputerShopContext context)
        {
            this.context = context;
        }

        public List<SupSum> TotalCostBySupplier(int supId)
        {
            /*var request = */
            return context.Supplier.Join(context.Product, s => s.id, p => p.id_manufacturer, (s, p) => new { SupId = s.id, SupName = s.name, ProdPrice = p.price, ProdCount = p.count }).Where(s => s.SupId == supId).GroupBy(s => s.SupName).Select(s => new SupSum() { SupName = s.Key, TotalCost = s.Sum(a => a.ProdPrice * a.ProdCount) }).ToList();


            //return request.Select(i => new SupSum { SupName = i.SupName, TotalCost = (decimal)i.TotalCost }).ToList();
        }

        public List<OrderFromTo> ExecuteSP(DateTime from, DateTime to)
        {

            SqlParameter param1 = new SqlParameter("@fr", from);
            SqlParameter param2 = new SqlParameter("@t", to);

            var request = context.Database.SqlQuery<OrderC>("OrdersFromTo @fr, @t", new object[] { param1, param2 }).ToList();

            return request.Select(i => new OrderFromTo
            {
                Id = i.id,
                Customer = (int)i.id_client,
                Date = i.date,
                Seller = (int)i.id_seller,
                TotalCost = i.total_cost,
                Products = String.Join(" , ", context.OrderC.Where(o => o.id == i.id).Join(context.Order_line, o => o.id, l => l.id_order, (o, l) => l).Join(context.Product, a => a.id_product, p => p.id, (a, p) => p.name))
            }).ToList();
        }
    }
}

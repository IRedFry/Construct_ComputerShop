using System;
using System.Linq;
using System.Collections.Generic;
using DAL;

namespace BLL
{
    public class ReportService : IReportService
    {
        private IDbRepos context;
        public ReportService(IDbRepos repos)
        {
            context = repos;
        }

        public SupSum TotalCostBySupplier(int supId)
        {
            return context.Reports.TotalCostBySupplier(supId).Select(i => new SupSum { SupName = i.SupName, TotalCost = i.TotalCost }).FirstOrDefault();     
 
        }

        public List<OrderFromTo> ExecuteSP(DateTime from, DateTime to)
        {
            return context.Reports.ExecuteSP(from, to).Select(i => new OrderFromTo { Customer = i.Customer, Date = i.Date, Id = i.Id, Products = i.Products, Seller = i.Seller, TotalCost = i.TotalCost }).ToList();
        }
    }
}

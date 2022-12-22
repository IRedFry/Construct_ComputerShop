using System;
using System.Collections.Generic;

namespace BLL
{
    public interface IReportService
    {
        SupSum TotalCostBySupplier(int supId);
        List<OrderFromTo> ExecuteSP(DateTime from, DateTime to);
    }
}

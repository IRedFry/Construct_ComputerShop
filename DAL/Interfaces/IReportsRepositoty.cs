using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IReportsRepositoty
    {
        List<SupSum> TotalCostBySupplier(int supId);
        List<OrderFromTo> ExecuteSP(DateTime from, DateTime to);
    }
}

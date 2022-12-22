using System.Collections.Generic;
using System.Linq;
using DAL;

namespace BLL
{
    public class RecommendationService : IRecommendationService
    {
        private IDbRepos context;

        public RecommendationService(IDbRepos repos)
        {
            context = repos;
        }

        public List<ProductModel> GetLastProducts()
        {
            return context.Products.GetList().Where(i => i.count == 1).Select(i => new ProductModel(i)).ToList();
        }

        public List<ProductModel> GetPopularProducts()
        {
            return context.OrderLines.GetList().Where(i => i.OrderC.id_status != 3).Join(context.Products.GetList(), orl => orl.id_product, p => p.id, (orl, p) =>
            p).GroupBy(p => p.id).OrderByDescending(p => p.Count()).Distinct().Take(5).Select(i => new ProductModel(context.Products.GetItem(i.Key))).ToList();
        }

        public List<ProductModel> GetRecommenProductsBasedOnCategory(int customer_id)
        {

            int? mostFreqCat = context.Orders.GetList().Where(o => o.id_client == customer_id).Join(context.OrderLines.GetList(), o => o.id, l => l.id_order, (o, l) =>
            l).Join(context.Products.GetList(), l => l.id_product, p => p.id, (l, p) => p).GroupBy(p => p.id_category).OrderByDescending(c => c.Count()).Select(g => g.Key).FirstOrDefault();

            if (mostFreqCat == null)
                return null;

            return context.Products.GetList().Where(p => p.id_category == mostFreqCat).Select(i => new ProductModel(i)).ToList();
        }
    }
}

using System.Collections.Generic;

namespace BLL
{
    public interface IRecommendationService
    {
        List<ProductModel> GetRecommenProductsBasedOnCategory(int customer_id);
        List<ProductModel> GetPopularProducts();
        List<ProductModel> GetLastProducts();
    }
}

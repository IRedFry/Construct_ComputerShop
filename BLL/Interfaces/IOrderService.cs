namespace BLL
{
    public interface IOrderService
    {
        bool MakeOrderFromModel(OrderModel orderDto);
        bool MakeOrderFromBusket(OrderModel cart);
        bool TakeOrder(int id);
        bool CancelOrder(int id);
        bool VerifyOrder(int id);

    }
}

using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class OrderService : IOrderService
    {
        private IDbRepos context;

        public OrderService(IDbRepos repos)
        {
            context = repos;
        }
        public bool CancelOrder(int id)
        {
            
            OrderC o = context.Orders.GetItem(id);
            /*
            if (o.id_status == 1)
            {

                foreach (var ol in o.Order_line)
                    ((OrderLineRepositorySQL)(context.OrderLines)).Delete(id, ol.id);

                context.Orders.Delete(id);

                if (context.Save() > 0)
                    return true;
                return false;
            }
            else if (o.id_status == 2)
            {
                foreach (var ol in o.Order_line)
                {
                    ((OrderLineRepositorySQL)(context.OrderLines)).Delete(id, ol.id);
                    context.Products.GetItem((int)ol.id_product).count += ol.count;
                }

                context.Orders.Delete(id);

                if (context.Save() > 0)
                    return true;
                return false;
            }
            */

            //if (o.id_status == 1)
            //{
            //    o.id_status = 4;

            //    if (context.Save() > 0)
            //        return true;
            //    return false;
            //}
            //else if (o.id_status == 2)
            //{
                foreach (var ol in o.Order_line)
                {
                    Product p = context.Products.GetItem((int)ol.id_product);
                    p.count += ol.count;
                    context.Products.Update(p);
                }

                o.id_status = 4;
                context.Orders.Update(o);
                if (context.Save() > 0)
                    return true;
                return false;
            //}

            //return false;
        }
        public bool MakeOrderFromBusket(OrderModel cart)
        {
            OrderC o = context.Orders.GetItem(cart.Id);
            o.id_status = 1;

            foreach (var ol in o.Order_line)
            {
                Product p = context.Products.GetItem((int)ol.id_product);         
                p.count -= ol.count;
                context.Products.Update(p);
            }

            context.Orders.Update(o);

            if (context.Save() > 0)
                return true;
            return false;
        }
        public bool MakeOrderFromModel(OrderModel orderDto)
        {
            List<Order_line> orderedProducts = new List<Order_line>();
            int lineid = 0;

            int id;
            if (orderDto.Id != -1)
                id = orderDto.Id;
            else
                id = context.Orders.GetList().OrderByDescending(i => i.id).FirstOrDefault()== null ? 0 : context.Orders.GetList().OrderByDescending(i => i.id).FirstOrDefault().id + 1;

            int sum = 0;
            for (int i = 0; i < orderDto.ProductsIds.Count; ++i)
            {
                Product p = context.Products.GetItem(orderDto.ProductsIds[i]);

                if (p == null)
                    throw new Exception("Продукт не найден");
                orderedProducts.Add(new Order_line {id = lineid++, id_product = p.id, Product = p, count = orderDto.ProductCounts[i],id_order = id});
                sum += (int)(p.price * orderDto.ProductCounts[i] * (1 - p.sale));
            }

            Seller s = context.Sellers.GetList().Where(i => i.id == orderDto.SellerId).FirstOrDefault();
            Customer c = context.Customers.GetList().Where(i => i.id == orderDto.CustomerId).FirstOrDefault();

            OrderC r = new OrderC
            {
                date = DateTime.Now,
                id_client = orderDto.CustomerId,
                id_seller = orderDto.SellerId,
                total_cost = sum,
                Order_line = orderedProducts,
                id = id,
                Seller = s,
                Customer = c,
                id_status = 1
            };

            context.Orders.Create(r);
            if (context.Save() > 0)
                return true;
            return false;
        }

        public bool TakeOrder(int id)
        {
            OrderC o = context.Orders.GetItem(id);
            o.id_status = 0;

            if (context.Save() > 0)
                return true;
            return false;
        }

        public bool VerifyOrder(int id)
        {
            OrderC o = context.Orders.GetItem(id);
            o.id_status = 2;

            if (context.Save() > 0)
                return true;
            return false;
        }
    }
}

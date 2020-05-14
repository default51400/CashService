using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.HelpersModels;
using Infrastructure.Models;
using Infrastructure.Models.Interfaces;

namespace Infrastructure.Helpers
{
    public class RepositoryHelper
    {
        public static async Task<List<CashOrder>> GetOrCreateOrders(List<CashOrder> ordersList, ICashOrderRepository db)
        {
            foreach (var order in ordersList)
            {
                if (RequestMode.IsCreate(order))
                {
                    order.Status = OrderStatus.Ready.ToString();
                    await db.CashOrderCreate(order);
                }
                else if (RequestMode.IsGetById(order))
                {
                    ordersList = new List<CashOrder>();
                    ordersList.Add(await db.CashOrderGetById(order.Id));
                    break;//temporary solution
                }
                else if (RequestMode.IsGetByUserIdAndOfficeAddress(order))
                {
                    ordersList = await db.CashOrderGetByUserIdAndOfficeAddress(order.UserId, order.OfficeAddress);
                    break;//temporary solution
                }
            }

            return ordersList;
        }
    }
}

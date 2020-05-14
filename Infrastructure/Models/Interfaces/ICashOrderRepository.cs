using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Models.Interfaces
{
    public interface ICashOrderRepository
    {
        Task<CashOrder> CashOrderCreate(CashOrder cashOrder);
        Task<CashOrder> CashOrderGetById(int id);
        Task<List<CashOrder>> CashOrderGetByUserIdAndOfficeAddress(string client, string address);
        Task<List<CashOrder>> CashOrderGetAll();
    }
}

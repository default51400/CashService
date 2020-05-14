using Infrastructure.Models;

namespace Infrastructure.Helpers
{
    public static class RequestMode
    {
        public static bool IsCreate(CashOrder order)
        {
            return (order.Id == 0 && order.Amount != 0
                    && !string.IsNullOrWhiteSpace(order.UserId)
                    && !string.IsNullOrWhiteSpace(order.OfficeAddress)
                    && !string.IsNullOrWhiteSpace(order.Currency));
        }
        public static bool IsGetById(CashOrder order)
        {
            return (order.Id != 0 && order.Amount == 0
                    && string.IsNullOrWhiteSpace(order.UserId)
                    && string.IsNullOrWhiteSpace(order.OfficeAddress)
                    && string.IsNullOrWhiteSpace(order.Currency));
        }

        public static bool IsGetByUserIdAndOfficeAddress(CashOrder order)
        {
            return (order.Id == 0 && order.Amount == 0
                    && !string.IsNullOrWhiteSpace(order.UserId)
                    && !string.IsNullOrWhiteSpace(order.OfficeAddress)
                    && string.IsNullOrWhiteSpace(order.Currency));
        }
    }
}

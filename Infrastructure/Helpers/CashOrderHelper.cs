using System.Collections.Generic;
using Infrastructure.HelpersModels;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Helpers
{
    public static class CashOrderHelper
    {
        public static void Validate(CashOrder cashOrder, ILogger logger, ModelStateDictionary modelState)
        {
            string message = "";

            if (cashOrder == null)
            {
                message = "cashOrder is null";
                logger.LogWarning(message);
                modelState.AddModelError("Other", message);
            }
            //Another validation
        }

        public static void AddClientIp(CashOrder cashOrder, string ipAddress)
        {
            cashOrder.IpAddress = ipAddress;
        }

        public static void SetStatusInProgress(CashOrder cashOrder)
        {
            cashOrder.Status = OrderStatus.InProgress.ToString();
        }

        public static IEnumerable<CashOrderResponse> ConvertToResponse(List<CashOrder> cashOrders)
        {
            List<CashOrderResponse> result = new List<CashOrderResponse>();

            foreach (var order in cashOrders)
            {
                CashOrderResponse response = new CashOrderResponse();
                response.Amount = order.Amount;
                response.Currency = order.Currency;
                response.Status = order.Status;
                result.Add(response);
            }

            return result;
        }
    }
}

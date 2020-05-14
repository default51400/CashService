using System.Collections.Generic;
using Infrastructure.HelpersModels;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Helpers
{
    public static class CashOrderHelper
    {
        /// <summary>
        /// CashOrder validation 
        /// </summary>
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

        /// <summary>
        /// Convert CashOrders to CashOrderResponseGet
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CashOrderResponseGet> ConvertToResponseGet(List<CashOrder> cashOrders)
        {
            List<CashOrderResponseGet> result = new List<CashOrderResponseGet>();

            foreach (var order in cashOrders)
            {
                CashOrderResponseGet response = new CashOrderResponseGet();
                response.Amount = order.Amount;
                response.Currency = order.Currency;
                response.Status = order.Status;
                result.Add(response);
            }

            return result;
        }

        /// <summary>
        /// Convert CashOrders to CashOrderResponseCreate
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CashOrderResponseCreate> ConvertToResponseCreate(List<CashOrder> cashOrders)
        {
            List<CashOrderResponseCreate> result = new List<CashOrderResponseCreate>();

            foreach (var order in cashOrders)
            {
                CashOrderResponseCreate response = new CashOrderResponseCreate();
                response.Id = order.Id;
                result.Add(response);
            }

            return result;
        }
    }
}

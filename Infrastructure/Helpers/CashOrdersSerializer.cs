using System;
using System.Collections.Generic;
using System.Text.Json;
using Infrastructure.Models;

namespace Infrastructure.Helpers
{
    public static class CashOrdersSerializer
    {
        /// <summary>
        /// Deserialize string to CashOrder
        /// </summary>
        /// <param name="json">serialized CashOrder</param>
        /// <returns>List<CashOrder></returns>
        public static List<CashOrder> Deserialize(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<List<CashOrder>>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Serialize CashOrder to string
        /// </summary>
        /// <param name="cashOrders"></param>
        /// <returns></returns>
        public static string Serialize(List<CashOrder> cashOrders)
        {
            try
            {
                return JsonSerializer.Serialize<List<CashOrder>>(cashOrders);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

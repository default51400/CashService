﻿using System.Text.Json.Serialization;

namespace Infrastructure.Models
{
    public class CashOrderResponse
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        public CashOrderResponse()
        {
        }

        public CashOrderResponse(CashOrder order)
        {
            this.Amount = order.Amount;
            this.Currency = order.Currency;
            this.Status = order.Status;
        }

        
    }
}
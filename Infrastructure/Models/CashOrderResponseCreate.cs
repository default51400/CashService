using System.Text.Json.Serialization;

namespace Infrastructure.Models
{
    public class CashOrderResponseCreate
    {
        [JsonPropertyName("order_id")]
        public int Id { get; set; }

        public CashOrderResponseCreate()
        {
        }

        public CashOrderResponseCreate(CashOrder order)
        {
            this.Id = order.Id;
        }
    }
}
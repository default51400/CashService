using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CashServiceAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class CashOrdersController : Controller
    {
        private IConfiguration configuration;
        private readonly ILogger logger;
        private Sender sender;

        public CashOrdersController(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(typeof(CashOrdersController));
            this.configuration = configuration;
        }

        /// <summary>
        /// API call Get by JSON: "request_id" or "client_id" && "departemnt_address"
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CashOrderResponseGet>>> Get(CashOrder cashOrder)
        {
            try
            {
                List<CashOrder> cashOrders = new List<CashOrder>();
                
                if (RequestMode.IsGetById(cashOrder) || RequestMode.IsGetByUserIdAndOfficeAddress(cashOrder))
                {
                    cashOrders = SendMessage(cashOrder);

                    return Ok(CashOrderHelper.ConvertToResponseGet(cashOrders)); 
                }             
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest();
            }

            return BadRequest();
        }
        
        /// <summary>
        /// API call POST create by JSON: "client_id", "departemnt_address", "amount", "currency"
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CashOrderResponseCreate>> Create(CashOrder cashOrder)
        {
            try
            {
                CashOrderHelper.Validate(cashOrder, logger, ModelState);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                CashOrderHelper.AddClientIp(cashOrder, HttpContext.Connection.RemoteIpAddress.ToString());
                CashOrderHelper.SetStatusInProgress(cashOrder);

                if (RequestMode.IsCreate(cashOrder))
                {
                    List<CashOrder> cashOrders = SendMessage(cashOrder);

                    if (cashOrders.Count > 0)
                        return Ok(CashOrderHelper.ConvertToResponseCreate(cashOrders));
                    else
                        return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + "\n" + ex.StackTrace);
                return BadRequest();
            }
            return BadRequest();
        }

        /// <summary>
        /// Send message to message queue
        /// </summary>
        private List<CashOrder> SendMessage(CashOrder cashOrder)
        {
            List<CashOrder> ordersList = new List<CashOrder>();
            ordersList.Add(cashOrder);
            string json = CashOrdersSerializer.Serialize(ordersList);

            try
            {
                sender = new Sender(configuration);
                var response = sender.Call(json);
                ordersList = CashOrdersSerializer.Deserialize(response);

                return ordersList;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                sender.Close();
            }
        }
    }
}
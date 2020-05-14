using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.Models;
using Infrastructure.Models.Interfaces;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class CashOrderRepository : ICashOrderRepository
    {
        private string connectionString = null;

        public CashOrderRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<CashOrder> CashOrderCreate(CashOrder cashOrder)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    db.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@UserId", cashOrder.UserId);
                    param.Add("@OfficeAddress", cashOrder.OfficeAddress);
                    param.Add("@Amount", cashOrder.Amount);
                    param.Add("@Currency", cashOrder.Currency);
                    param.Add("@IpAddress", cashOrder.IpAddress);
                    param.Add("@Status", cashOrder.Status);

                    var result = await db.QueryAsync("CashOrderCreate", param, commandType: CommandType.StoredProcedure);

                    //it needs for async return Id
                    var firstRecord = result.FirstOrDefault() as IDictionary<string, object>;
                    cashOrder.Id = Convert.ToInt32(firstRecord["Id"]);
                    return cashOrder;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CashOrder>> CashOrderGetAll()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    db.Open();

                    var result = await db.QueryAsync<CashOrder>("CashOrderGetAll", commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CashOrder> CashOrderGetById(int id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    db.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id);

                    var result = await db.QueryAsync<CashOrder>("CashOrderGetById", param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<CashOrder>> CashOrderGetByUserIdAndOfficeAddress(string userId, string officeAddress)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    db.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@UserId", userId);
                    param.Add("@OfficeAddress", officeAddress);

                    var result = await db.QueryAsync<CashOrder>("CashOrderGetByUserIdAndOfficeAddress", param, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
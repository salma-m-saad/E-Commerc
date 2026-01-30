using Ecom.core.Entities;
using Ecom.core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositires
{
    class CustomerBasketRepositry : ICustomerBasketRepositry
    {
        private readonly IDatabase _database;

        public CustomerBasketRepositry(IConnectionMultiplexer redis)
        {
            _database=redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
            
        }

        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var result =await _database.StringGetAsync(id);
            if (!string.IsNullOrEmpty(result)) 
            
                return JsonSerializer.Deserialize<CustomerBasket>(result);

            return null;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var _basket =await _database.StringSetAsync( customerBasket.Id, JsonSerializer.Serialize(customerBasket),TimeSpan.FromDays(5));
            if (_basket) return await GetBasketAsync(customerBasket.Id);
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using StackExchange.Redis;


namespace Ecom.infrastructure.Repositires
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConnectionMultiplexer redis;
        private readonly IImageManagementServicecs _imageManagementServicecs;


        public ICategoryRepositry CategoryRepositry { get; }

        public IProductRepositry ProductRepositry { get; }

        public ICustomerBasketRepositry customerBasketRepositry { get; }

        public UnitOfWork(AppDbContext context, IImageManagementServicecs imageManagementServicecs, IMapper mapper,IConnectionMultiplexer redis)
        {
            _context = context;
            _imageManagementServicecs = imageManagementServicecs;
            _mapper = mapper;
            this.redis = redis;
            CategoryRepositry = new CategoryRepositry(_context);
            ProductRepositry = new ProductRepositry(_context,imageManagementServicecs,mapper);
            customerBasketRepositry = new CustomerBasketRepositry(redis);

        }

    }
}

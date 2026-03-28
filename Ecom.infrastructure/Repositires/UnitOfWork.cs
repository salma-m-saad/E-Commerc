using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecom.core.Entities;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;


namespace Ecom.infrastructure.Repositires
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConnectionMultiplexer redis;
        private readonly IImageManagementServicecs _imageManagementServicecs;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<AppUser> signManager;
        private readonly IGenerateToken _generateToken;
        public ICategoryRepositry CategoryRepositry { get; }

        public IProductRepositry ProductRepositry { get; }

        public ICustomerBasketRepositry customerBasketRepositry { get; }

        public IAuth Auth { get; }

        public UnitOfWork(AppDbContext context, IImageManagementServicecs imageManagementServicecs, IMapper mapper, IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signManager, IGenerateToken generateToken)
        {
            _context = context;
            _imageManagementServicecs = imageManagementServicecs;
            _mapper = mapper;
            this.redis = redis;
            this.userManager = userManager;
            _emailService = emailService;
            this.signManager = signManager;
            _generateToken = generateToken;
            CategoryRepositry = new CategoryRepositry(_context);
            ProductRepositry = new ProductRepositry(_context, imageManagementServicecs, mapper);
            customerBasketRepositry = new CustomerBasketRepositry(redis);
            Auth = new AuthRepositry(userManager, emailService, signManager,_generateToken);

        }

    }
}

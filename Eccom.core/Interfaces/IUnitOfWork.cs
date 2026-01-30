using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Interfaces
{
    public interface IUnitOfWork
    {
        public ICategoryRepositry CategoryRepositry { get; }
        public IProductRepositry ProductRepositry { get; }
        public ICustomerBasketRepositry customerBasketRepositry { get; }
    }
}

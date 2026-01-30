using Ecom.core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Sharing
{
    public class ProductParams
    {
        // string? sort,int? CategoryID, int PageSize, int PageNum
        public string sort;

        public int? CategoryID;
        public int PageNum { get; set; } = 1;

        public string? Search { get; set; }

        public int MaxSize = 6;

        private int _PageSize=3;
        public int PageSize 
        {
            get { return _PageSize; }
            set { _PageSize = value > MaxSize ? MaxSize : value; }
        }


    }
}

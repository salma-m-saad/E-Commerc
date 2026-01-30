using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Entities.Product
{
    public class Category:BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        //initialize collection to avoid null reference exception
        //public ICollection<Product> Products { get; set; }=new HashSet< Product > ();
    }
}

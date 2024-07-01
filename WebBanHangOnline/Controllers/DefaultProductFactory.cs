using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class DefaultProductFactory:ProductFactory
    {
        private readonly ApplicationDbContext _context;

        public DefaultProductFactory(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Product> GetProducts()
        {
            return _context.Products.Where(p => p.Quantity > 0).ToList();
        }

    }
}

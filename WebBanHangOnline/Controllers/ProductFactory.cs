using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ProductFactory
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ProductFactory(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public List<Product> GetFilteredProducts(bool isSaleOnly = true)
        {
            IQueryable<Product> query = db.Products;

            if (isSaleOnly)
            {
                query = query.Where(x => x.IsSale && x.IsActive && x.Quantity > 0);
            }
            else
            {
                query = query.Where(x => x.IsActive && x.Quantity > 0);
            }

            return query.ToList();
        }
    }
}
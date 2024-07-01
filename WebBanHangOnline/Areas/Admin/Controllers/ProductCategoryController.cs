using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public sealed class ProductCategoryRepository
        {
            private static readonly ApplicationDbContext context = new ApplicationDbContext();
            private static ProductCategoryRepository instance = null;
            private static readonly object padlock = new object();

            ProductCategoryRepository() { }

            public static ProductCategoryRepository Instance
            {
                get
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ProductCategoryRepository();
                        }
                        return instance;
                    }
                }
            }


            public IEnumerable<ProductCategory> GetAllCategories()
            {
                return context.ProductCategories.ToList();
            }

            // Thêm các phương thức khác nếu cần
        }

        public ActionResult Index()
        {
            var repository = ProductCategoryRepository.Instance;
            var categories = repository.GetAllCategories();
            return View(categories);
        }
        public interface IProductObserver
        {
            void ProductAdded(ProductCategory product);
        }

        public class ProductCategoryRepository2
        {
            private readonly List<IProductObserver> _observers = new List<IProductObserver>();
            private readonly ApplicationDbContext _context = new ApplicationDbContext();

            public void Attach(IProductObserver observer)
            {
                _observers.Add(observer);
            }

            public void Detach(IProductObserver observer)
            {
                _observers.Remove(observer);
            }

            public void Notify(ProductCategory product)
            {
                foreach (var observer in _observers)
                {
                    observer.ProductAdded(product);
                }
            }

            public void AddProduct(ProductCategory model)
            {
                if (model != null)
                {
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now;
                    model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);

                    _context.ProductCategories.Add(model);
                    _context.SaveChanges();
                }
            }

            // Method to send notification after product addition
            public void NotifyObserversAboutAddedProduct(ProductCategory model)
            {
                Notify(model);
            }
        }

        public class ProductObserver : IProductObserver
        {
            public void ProductAdded(ProductCategory product)
            {
                // Thực hiện các hành động khi có sản phẩm mới được thêm vào
                // Ví dụ: Gửi thông báo, cập nhật giao diện, logging, v.v.
                Console.WriteLine($"Sản phẩm mới đã được thêm vào: {product.Title}");
            }
        }

  
        private readonly ProductCategoryRepository2 _productCategoryRepository = new ProductCategoryRepository2();
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                _productCategoryRepository.AddProduct(model);
                _productCategoryRepository.NotifyObserversAboutAddedProduct(model);
                return RedirectToAction("Index");
            }
            return View();
        }



        public ActionResult Edit(int id)
        {
            var item = db.ProductCategories.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                db.ProductCategories.Attach(model);
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.Entry(model).Property(x => x.Title).IsModified = true;
                db.Entry(model).Property(x => x.Description).IsModified = true;
               
                db.Entry(model).Property(x => x.Alias).IsModified = true;
                db.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(model).Property(x => x.SeoTitle).IsModified = true;
               
                db.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(model).Property(x => x.Modifiedby).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids)) // lấy chuổi id cần xóa
            {
                var items = ids.Split(','); // cắt chuổi 
                if (items != null && items.Any())// kiểm tra nếu chuổi tồn tại 
                {
                    foreach (var item in items)
                    {
                        var obj = db.ProductCategories.Find(Convert.ToInt32(item));// chuyển đổi chuổi thành số
                        db.ProductCategories.Remove(obj); // xóa hàng loạt
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
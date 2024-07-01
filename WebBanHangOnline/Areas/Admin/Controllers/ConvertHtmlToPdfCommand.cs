using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ConvertHtmlToPdfCommand : ICommand
    {
        private readonly List<Order> _items;
        private readonly ControllerContext _controllerContext;

        public ConvertHtmlToPdfCommand(List<Order> items, ControllerContext controllerContext)
        {
            _items = items;
            _controllerContext = controllerContext;
        }

        public void Execute()
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;

            var htmlPdf = ViewRenderHelper.RenderPartialToString(_controllerContext, "~/Areas/Admin/Views/Order/PartialViewPdfResult.cshtml", _items);

            PdfDocument doc = converter.ConvertHtmlString(htmlPdf);
            string fileName = string.Format("{0}.pdf", DateTime.Now.Ticks);
            string pathFile = string.Format("{0}/{1}", _controllerContext.HttpContext.Server.MapPath("~/Resource/Pdf"), fileName);

            doc.Save(pathFile);
            doc.Close();
        }

    }

}
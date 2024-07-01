using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class RenderPartialToStringCommand : ICommand
    {
        private string _viewName;
        private object _model;
        private ControllerContext _controllerContext;
        public string RenderedHtml { get; private set; }

        public RenderPartialToStringCommand(string viewName, object model, ControllerContext controllerContext)
        {
            _viewName = viewName;
            _model = model;
            _controllerContext = controllerContext;
        }

        public void Execute()
        {
            if (string.IsNullOrEmpty(_viewName))
                _viewName = _controllerContext.RouteData.GetRequiredString("action");
            ViewDataDictionary ViewData = new ViewDataDictionary();
            TempDataDictionary TempData = new TempDataDictionary();
            ViewData.Model = _model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(_controllerContext, _viewName);
                ViewContext viewContext = new ViewContext(_controllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                RenderedHtml = sw.ToString();
            }
        }
    }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
        }
    }

}
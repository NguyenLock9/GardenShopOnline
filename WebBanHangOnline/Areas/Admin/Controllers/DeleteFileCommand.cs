using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class DeleteFileCommand : ICommand
    {
        private readonly string _fileName;
        private string _resultMessage;

        public DeleteFileCommand(string fileName)
        {
            _fileName = fileName;
        }

        public void Execute()
        {
            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Resource/Pdf/") + _fileName;

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    _resultMessage = "File deleted successfully";
                }
                else
                {
                    _resultMessage = "File not found";
                }
            }
            catch (Exception ex)
            {
                _resultMessage = "Error deleting file: " + ex.Message;
                // Có thể ghi log chi tiết ngoại lệ để gỡ lỗi tại đây
            }

        }
        public string GetResultMessage()
        {
            return _resultMessage;
        }
    }

}
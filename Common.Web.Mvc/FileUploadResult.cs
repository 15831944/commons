using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System.Web.Mvc
{
    public class FileUploadResult : ActionResult
    {
        public const string ResponseContentType = "text/plain";

        #region Properties

        public string Message { get; set; }

        public bool Success { get; set; }
        public JObject Data { get; set; }

        #endregion Properties

        public FileUploadResult(bool success, string message, JObject data)
        {
            this.Message = message;
            this.Success = success;
            this.Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = ResponseContentType;

            response.Write(BuildResponse());
        }

        private string BuildResponse()
        {
            var response = new JObject
            {
                { "success", this.Success },
                { "message", this.Message },
                { "data", this.Data }
            };
            return response.ToString();
        }
    }
}
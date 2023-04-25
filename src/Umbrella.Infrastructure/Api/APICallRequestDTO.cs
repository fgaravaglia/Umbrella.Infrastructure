using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.Infrastructure.Api
{
    public class APICallRequestDTO
    {
        public string Url { get; set; }
        public string MethodName { get; set; }
        public Dictionary<string, string> RequestHeader { get; set; }
        public object? RequestObject { get; set; }
        public string ContentType { get; set; }
        public string Token { get; set; }

        public APICallRequestDTO()
        {
            ContentType = ContentTypes.ApplicationJson;
            Token = string.Empty;
            RequestHeader = new Dictionary<string, string>();
            Url = "";
            MethodName = "";
        }
    }
}
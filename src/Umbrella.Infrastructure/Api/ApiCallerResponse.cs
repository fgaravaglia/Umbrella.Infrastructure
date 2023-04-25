using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbrella.Infrastructure.Api
{
    public class APICallResponseDTO<T> where T : class
    {
        public bool IsSucceded { get { return StatusCode == 200 || StatusCode == 201; } }

        public int StatusCode { get; set; }
        public T? Body { get; set; }
        public string Error { get; set; }

        public APICallResponseDTO()
        {
            StatusCode = 200;
            Error = "";
        }
    }
}
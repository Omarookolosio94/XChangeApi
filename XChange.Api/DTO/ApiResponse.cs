using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class ApiResponse
    {
        public ApiResponse(int code, string message, string details = null)
        {
            Code = code;
            Message = message;
            Details = details;
        }

        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }

        //Convert Api Error to Json format
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

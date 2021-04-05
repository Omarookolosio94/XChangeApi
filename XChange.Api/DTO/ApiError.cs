using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class ApiError
    {
        public ApiError(int errorCode , string errorMessage , string errorDetails = null)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorDetails = errorDetails;
        }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }

        //Convert Api Error to Json format
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
} 

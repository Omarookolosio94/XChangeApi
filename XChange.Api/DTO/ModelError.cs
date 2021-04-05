using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class ModelError
    {

        public ModelError(int errorCode, string errorMessage ,List<Error> errorDetails)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorDetails = errorDetails;
        }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public List<Error> ErrorDetails { get; set; }


        //Convert Api Error to Json format
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public class Error{
            public string modelName { get; set; }
            public string modelErrorMessgae { get; set; }
        }
    }
}

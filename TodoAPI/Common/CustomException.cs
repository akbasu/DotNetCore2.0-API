using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace TodoAPI.Common
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Description { get; }

        public CustomException(string message, string description, HttpStatusCode httpStatusCode) : base(message)
        {
            StatusCode = httpStatusCode;            
            Description = description;
        }
    }

    public class InvalidInputException : CustomException
    {
        private const string MSG = "Invalid input data";
        public InvalidInputException(string description) : base(MSG, description, HttpStatusCode.BadRequest)
        {
        }
    }

    public class ServerException : CustomException
    {
        private const string MSG = "Server processing failed";
        public ServerException(string description) : base(MSG, description, HttpStatusCode.InternalServerError)
        {
        }
    }
}

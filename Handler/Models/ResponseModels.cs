using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Handler
{
    public class ResponseModel
    {
        public bool   Success { get; set; }
        public string Error   { get; set; }

        public CourseItem[] Data { get; set; }

        public RequestHeader[] RequestHeaders { get; set; }

        public ResponseModel()
        {
        }

        public ResponseModel(string error, IHeaderDictionary headers)
        {
            Success        = false;
            Error          = error;
            RequestHeaders = headers.Select(p => new RequestHeader { Name = p.Key, Value = p.Value.ToString() }).ToArray();
        }

        public ResponseModel(CourseItem[] data, IHeaderDictionary headers)
        {
            Success = true;
            Data    = data;

            RequestHeaders = headers.Select(p => new RequestHeader { Name = p.Key, Value = p.Value.ToString() }).ToArray();
        }
    }

    public sealed class RequestHeader
    {
        public string Name  { get; set; }
        public string Value { get; set; }
    }
}
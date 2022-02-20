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
        
        public object RequestHeaders { get; set; }

        public ResponseModel()
        {
            
        }
        
        public ResponseModel(string error, IHeaderDictionary headers)
        {
            Success        = false;
            Error          = error;
            RequestHeaders = headers.Select(p => new { Name = p.Key, Value = p.Value.ToString() });
        }

        public ResponseModel(CourseItem[] data, IHeaderDictionary headers)
        {
            Success = true;
            Data    = data;

            RequestHeaders = headers.Select(p => new { Name = p.Key, Value = p.Value.ToString() });
        }
    }
}
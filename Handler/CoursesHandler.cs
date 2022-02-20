using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Handler
{
    public class CoursesHandler
    {
        readonly HttpClient client;

        const string URL = "https://www.cbr-xml-daily.ru/daily_json.js";

        public CoursesHandler()
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;

            client = new HttpClient(httpHandler);
        }

        public async Task<CourseItem[]> Get(params string[] codes)
        {
            var response = await client.GetFromJsonAsync<IncomingCourseResponse>(URL);
            return response.Valute.Where(p => codes.Contains(p.Key))
                           .Select(p => new CourseItem()
                           {
                               Code  = p.Value.CharCode,
                               Name  = p.Value.Name,
                               Value = p.Value.Value
                           })
                           .ToArray();
        }

        class IncomingCourseResponse
        {
            public Dictionary<string, IncomingCourseItem> Valute { get; set; }
        }

        class IncomingCourseItem
        {
            public string ID       { get; set; }
            public string NumCode  { get; set; }
            public string CharCode { get; set; }
            public string Name     { get; set; }
            public double Value    { get; set; }
        }
    }
}
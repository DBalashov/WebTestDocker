using System.Globalization;
using System.Linq;
using Handler;
using Newtonsoft.Json;

namespace WebTest.Models
{
    public class LogItemModel
    {
        public string DT   { get; }
        public string Host { get; }
        public string Data { get; }

        public LogItemModel(DBLogItem p)
        {
            DT   = p.dt.ToString("dd.MM.yyyy HH:mm:ss");
            Host = p.host;

            var m = JsonConvert.DeserializeObject<ResponseModel>(p.data);
            Data = string.Join(
                ", ", m.Data?.Select(d => "<span class='code'>" + d.Code + "</span>=<span class='value'>" + d.Value.ToString("F2", CultureInfo.InvariantCulture) + "</span>"));
        }
    }
}
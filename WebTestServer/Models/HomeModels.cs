using System.Collections.Generic;

#pragma warning disable CS8618

namespace WebTest.Models
{
    public class HomeIndexModel
    {
        public Dictionary<string, string> Env            { get; set; }
        public NetworkInterfaceItem[]     Interfaces     { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
    }

    public class NetworkInterfaceItem
    {
        public string Name      { get; set; }
        public string Addresses { get; set; }
    }
}
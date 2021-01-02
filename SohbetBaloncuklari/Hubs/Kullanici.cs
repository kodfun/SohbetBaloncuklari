using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SohbetBaloncuklari.Hubs
{
    public class Kullanici
    {
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public string TakmaAd { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
    }
}

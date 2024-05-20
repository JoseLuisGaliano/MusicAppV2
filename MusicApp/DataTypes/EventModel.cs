using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.DataTypes
{
    public class EventModel
    {
        public int EventID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
    }
}

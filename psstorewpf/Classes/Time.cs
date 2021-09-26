using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psstorewpf.Classes
{
    class Time
    {
        public int minutes { get; set; }
        public string text { get; set; }

        public static List<Time> timeList = new List<Time>()
            {
                new Time{ text="00:30",minutes=30 },
                new Time{ text="01:00",minutes=60 },
                new Time{ text="01:30",minutes=90 },
                new Time{ text="02:00",minutes=120 },
                new Time{ text="02:30",minutes=150 },
                new Time{ text="03:00",minutes=180 },
                new Time{ text="03:30",minutes=210 },
                new Time{ text="04:00",minutes=240 },
                new Time{ text="04:30",minutes=270 },
                new Time{ text="05:00",minutes=300 }
            };
        public static int stringTimeToMinutes(int selectedIndex)
        {
            return timeList[selectedIndex].minutes;
        }
    }
}

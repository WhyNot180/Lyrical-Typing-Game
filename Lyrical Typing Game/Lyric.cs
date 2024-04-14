using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrical_Typing_Game
{
    public class Lyric
    {
        [Index(0)]
        public string Words { get; set; }

        [Index(1)]
        public double TimeStamp { get; set; }
    }
}

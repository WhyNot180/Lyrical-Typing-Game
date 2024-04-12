using CsvHelper.Configuration.Attributes;
using System;

namespace Lyrical_Typing_Game
{
    public class Score
    {
        [Index(0)]
        public string PlayerName { get; set; }

        [Index(1)]
        public int HighScore { get; set; }

        [Index(2)]
        public double WordsPerMinute { get; set; }

        [Index(3)]
        public int Errors { get; set; }
    }
}

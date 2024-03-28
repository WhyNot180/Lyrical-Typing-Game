using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrical_Typing_Game
{
    public class Song
    {
        public string Name { get; }

        public Queue<(string, float)> Lyrics { get; } = new Queue<(string, float)>();

        

        public Song(string name) 
        {
            Name = name;
            Lyrics.Enqueue(("words", 10.0f));
            Lyrics.Enqueue(("More words", 15.0f));
        }
    }
}

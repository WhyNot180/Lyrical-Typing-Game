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

        public Queue<string> Lyrics { get; } = new Queue<string>();

        public Song(string name) 
        {
            Name = name;
            Lyrics.Enqueue("words");
            Lyrics.Enqueue("More words");
        }
    }
}

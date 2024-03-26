using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrical_Typing_Game
{
    public class Level
    {

        private StringBuilder currentWord = new StringBuilder(32);

        public string Name { get; }
        public Level(string name) 
        {
            Name = name;

        }


    }
}

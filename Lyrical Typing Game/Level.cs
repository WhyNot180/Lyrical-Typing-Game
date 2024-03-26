using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lyrical_Typing_Game
{
    public class Level
    {

        private StringBuilder currentWord = new StringBuilder(32);

        private int currentCharacter = 0;

        private int score = 0;

        private int errors = 0;

        public string Name { get; }
        public Level(string name) 
        {
            Name = name;

            Game1.gameWindow.TextInput += TextInputHandler;

        }

        private void TextInputHandler(object sender, TextInputEventArgs e)
        {
            if (e.Character == currentWord[currentCharacter])
            {
                currentCharacter++;
            } else
            {
                errors++;
            }

            if (currentCharacter == currentWord.Length)
            {
                currentWord.Clear();
                currentWord.Append();
            }
        }
    }
}

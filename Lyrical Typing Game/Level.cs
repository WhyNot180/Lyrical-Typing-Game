using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lyrical_Typing_Game
{
    public class Level
    {

        private StringBuilder currentWord = new StringBuilder(32);

        private int currentCharacter = 0;

        private int score = 0;

        private int errors = 0;

        private Song song;

        private bool end = false;

        public Level(Song song) 
        {
            this.song = song;
            currentWord.Append(song.Lyrics.Dequeue());
            Game1.gameWindow.TextInput += TextInputHandler;

        }

        public void Update()
        {
            if (end)
            {
                
            }
        }

        private void TextInputHandler(object sender, TextInputEventArgs e)
        {
            if (e.Character == currentWord[currentCharacter])
            {
                Debug.WriteLine(e.Character);
                currentCharacter++;
            } else if (char.IsLetterOrDigit(e.Character))
            {
                errors++;
            }
            Debug.WriteLine(currentCharacter);
            Debug.WriteLine(currentWord.Length);
            if (currentCharacter == currentWord.Length)
            {
                currentCharacter = 0;
                currentWord.Clear();
                if (song.Lyrics.Any())
                {
                    currentWord.Append(song.Lyrics.Dequeue());
                } else
                {
                    end = true;
                    Game1.gameWindow.TextInput -= TextInputHandler;
                }
            }
        }
    }
}

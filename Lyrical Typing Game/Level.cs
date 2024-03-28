﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        public bool End { get { return end; } } 

        private float currentTime = 0.0f;
        private float timeStamp;

        public Level(Song song) 
        {
            this.song = song;

            (string, float) initialLyric = song.Lyrics.Dequeue();
            currentWord.Append(initialLyric.Item1);
            timeStamp = initialLyric.Item2;
            Game1.gameWindow.TextInput += TextInputHandler;


        }

        public void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTime >= timeStamp) AdvanceLyric();
        }

        private void AdvanceLyric()
        {
            currentCharacter = 0;
            currentWord.Clear();
            if (song.Lyrics.Any())
            {
                (string, float) lyric = song.Lyrics.Dequeue();
                currentWord.Append(lyric.Item1);
                timeStamp = lyric.Item2;
                Debug.WriteLine(currentWord);
            }
            else
            {
                Game1.gameWindow.TextInput -= TextInputHandler;
                end = true;
            }
        }

        private void TextInputHandler(object sender, TextInputEventArgs e)
        {
            
            if (currentCharacter == currentWord.Length)
            {
                errors++;
                return;
            }
            if (e.Character == currentWord[currentCharacter])
            {
                Debug.WriteLine(e.Character);
                currentCharacter++;
            } else if (char.IsLetterOrDigit(e.Character))
            {
                errors++;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lyrical_Typing_Game
{
    public class Level
    {

        private SpriteFont Font;

        private StringBuilder currentWord = new StringBuilder(32);

        private int currentCharacter = 0;

        private int score = 0;

        private int errors = 0;

        private Song song;

        private bool finishedWord = false;

        private bool end = false;
        public bool End { get { return end; } } 

        private double currentTime = 0.0f;
        private double timeStamp;
        private double startTime = 0.0f;

        private double averageWordsPerMinute;

        public Level(Song song, ContentManager Content) 
        {
            this.song = song;

            (string, float) initialLyric = song.Lyrics.Dequeue();
            currentWord.Append(initialLyric.Item1);
            timeStamp = initialLyric.Item2;
            Game1.gameWindow.TextInput += TextInputHandler;

            Font = Content.Load<SpriteFont>("Lyric Font");


        }

        public void Start()
        {
            song.Play();
        }

        public void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTime >= timeStamp && !end) AdvanceLyric();
        }

        private void AdvanceLyric()
        {
            if (!finishedWord) averageWordsPerMinute += calculateWPM(currentTime - startTime);
            currentCharacter = 0;
            currentWord.Clear();
            if (song.Lyrics.Any())
            {
                (string, float) lyric = song.Lyrics.Dequeue();
                currentWord.Append(lyric.Item1);
                startTime = currentTime;
                timeStamp = lyric.Item2;
                finishedWord = false;
            }
            else
            {
                Game1.gameWindow.TextInput -= TextInputHandler;
                averageWordsPerMinute /= song.Count;
                averageWordsPerMinute = Math.Round(averageWordsPerMinute, 2);
                end = true;
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {

            // Draw current word
            _spriteBatch.DrawString(Font, currentWord, new Vector2(100,100), Color.DarkViolet, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);

            // Draw number of errors
            _spriteBatch.DrawString(Font, $"Errors: {errors}", new Vector2(0, 0), Color.DarkRed, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);

            // Draw amount of word written
            _spriteBatch.DrawString(Font, currentWord.ToString(0, currentCharacter), new Vector2(100, 200), Color.Green, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);

            if (end)
            {
                _spriteBatch.DrawString(Font, $"Average words per minute: {averageWordsPerMinute}", new Vector2(300, 0), Color.Black, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
            }
        }

        private void TextInputHandler(object sender, TextInputEventArgs e)
        {
            
            if (currentCharacter == currentWord.Length)
            {
                return;
            }
            if (e.Character == currentWord[currentCharacter])
            {
                currentCharacter++;
                if (currentCharacter == currentWord.Length)
                {
                    averageWordsPerMinute += calculateWPM(currentTime - startTime);
                    finishedWord = true;
                }
                
            } else if (char.IsLetterOrDigit(e.Character))
            {
                errors++;
            }

        }

        private double calculateWPM(double elapsedSeconds)
        {
            double numberOfWords = (currentCharacter + 1) / 5;

            double wordsPerMinute = numberOfWords / (elapsedSeconds / 60);

            return wordsPerMinute;
        }
    }
}

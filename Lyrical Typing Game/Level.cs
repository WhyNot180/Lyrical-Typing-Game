﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lyrical_Typing_Game
{
    public class Level
    {

        private SpriteFont Font;

        private StringBuilder currentWord = new StringBuilder(32);

        private int currentCharacter = 0;

        private Score score;

        private Song song;

        private bool finishedWord = false;

        private bool end = false;
        public bool End { get { return end; } } 

        private double currentTime = 0.0f;
        private double timeStamp;
        private double startTime = 0.0f;

        /// <summary>
        /// Creates a level based off of a Song
        /// </summary>
        /// <param name="song">The song used to construct the level</param>
        /// <param name="playerName">The name of the player playing the level</param>
        /// <param name="Content">The game's content manager</param>
        public Level(Song song, string playerName, ContentManager Content) 
        {
            this.song = song;

            score = new Score { Errors = 0, HighScore = 0, WordsPerMinute = 0, PlayerName = playerName };

            // Setup first lyric (word, timestamp)
            Lyric initialLyric = song.LyricsQueue.Dequeue();
            currentWord.Append(initialLyric.Words);
            timeStamp = initialLyric.TimeStamp;

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
            // If player fails to finish lyric on time, use whatever they finished to calculate WPM
            if (!finishedWord) score.WordsPerMinute += calculateWPM(currentTime - startTime);

            currentCharacter = 0;
            currentWord.Clear();

            // Check for end of song
            if (song.LyricsQueue.Any())
            {
                Lyric lyric = song.LyricsQueue.Dequeue();
                currentWord.Append(lyric.Words);
                startTime = currentTime;
                timeStamp = lyric.TimeStamp;
                finishedWord = false;
            }
            else
            {
                Game1.gameWindow.TextInput -= TextInputHandler;

                // Average the words per minute
                score.WordsPerMinute /= song.LyricsCount;
                score.WordsPerMinute = Math.Round(score.WordsPerMinute, 2);

                // Get name matched score, save to file the best stats achieved

                var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true
                };

                Score previousScore = CsvCrud<Score>.Read($"score\\{song.Name}.csv", config).FirstOrDefault(x => x.PlayerName.Equals(score.PlayerName), null);

                if (previousScore != null)
                {
                    score.Errors = score.Errors < previousScore.Errors ? score.Errors : previousScore.Errors;
                    score.WordsPerMinute = score.WordsPerMinute < previousScore.WordsPerMinute ? score.WordsPerMinute : previousScore.WordsPerMinute;
                    score.HighScore = score.HighScore < previousScore.HighScore ? score.HighScore : previousScore.HighScore;
                }

                CsvCrud<Score>.Update($"score\\{song.Name}.csv", config, x => x.PlayerName.Equals(score.PlayerName), score);

                end = true;
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {

            // Draw current word
            _spriteBatch.DrawString(Font, currentWord, new Vector2(100,100), Color.DarkViolet, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);

            // Draw number of errors
            _spriteBatch.DrawString(Font, $"Errors: {score.Errors}", new Vector2(0, 0), Color.DarkRed, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);

            // Draw amount of word written
            _spriteBatch.DrawString(Font, currentWord.ToString(0, currentCharacter), new Vector2(100, 200), Color.Green, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);

            if (end)
            {
                // Draw average words per minute
                _spriteBatch.DrawString(Font, $"Average words per minute: {score.WordsPerMinute}", new Vector2(300, 0), Color.Black, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
            }
        }

        private void TextInputHandler(object sender, TextInputEventArgs e)
        {
            // Guard clause if reached the end of word
            if (currentCharacter == currentWord.Length)
            {
                return;
            }

            if (e.Character == currentWord[currentCharacter])
            {
                currentCharacter++;
                if (currentCharacter == currentWord.Length)
                {
                    score.WordsPerMinute += calculateWPM(currentTime - startTime);
                    score.HighScore += 20;
                    finishedWord = true;
                }
                
            } else if (char.IsLetterOrDigit(e.Character))
            {
                score.Errors++;
            }

        }

        /// <summary>
        /// Calculates words per minute assuming each word is 5 characters long
        /// </summary>
        /// <param name="elapsedSeconds"></param>
        /// <returns>Words per minute</returns>
        private double calculateWPM(double elapsedSeconds)
        {
            double numberOfWords = (currentCharacter + 1) / 5;

            double wordsPerMinute = numberOfWords / (elapsedSeconds / 60);

            return wordsPerMinute;
        }
    }
}

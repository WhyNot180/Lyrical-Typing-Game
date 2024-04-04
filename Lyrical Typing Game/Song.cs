using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using NVorbis.Ogg;
using NVorbis;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Lyrical_Typing_Game
{
    public class Song
    {
        public string Name { get; }
        
        /// <summary>
        /// Stores lyrics as string with timestamp of when the lyric is no longer shown
        /// </summary>
        public Queue<(string, double)> Lyrics { get; } = new Queue<(string, double)>();

        /// <summary>
        /// Count of Lyrics excluding empty lines
        /// </summary>
        public int LyricsCount { get; private set; }

        private Microsoft.Xna.Framework.Media.Song audio;

        /// <summary>
        /// Song with timestamped lyrics and audio
        /// </summary>
        /// <param name="name"></param>
        /// <param name="audioFile">Name of audio file</param>
        /// <param name="csvFile">Comma separated values with a maximum of 2 fields, one string, one float</param>
        /// <param name="Content"></param>
        /// <exception cref="Exception"></exception>
        public Song(string name, string audioFile, string csvFile, ContentManager Content) 
        {
            Name = name;

            using (TextFieldParser parser = new TextFieldParser(csvFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;
                int currentRow = 0;
                while (!parser.EndOfData)
                {
                    //Process row
                    currentRow++;
                    string[] fields = parser.ReadFields();
                    if (fields.Length != 2 )
                    {
                        throw new Exception($"too many fields in {csvFile} on line {currentRow}");
                    }
                    string currentLyric = string.Empty;
                    foreach (string field in fields)
                    {
                        double timeStampSeconds;
                        if (double.TryParse(field, out timeStampSeconds)) {
                            Lyrics.Enqueue((currentLyric, timeStampSeconds));
                        } else
                        {
                            currentLyric = field;
                            if (!currentLyric.Equals(string.Empty)) LyricsCount++;
                        }
                    }
                }
            }

            audio = Content.Load<Microsoft.Xna.Framework.Media.Song>(audioFile);
        }

        /// <summary>
        /// Song with timestamped lyrics and audio
        /// </summary>
        /// <param name="name">The same name as the audio file</param>
        /// <param name="csvFile">Comma separated values with a maximum of 2 fields, one string, one float</param>
        /// <param name="Content"></param>
        public Song(string name, string csvFile, ContentManager Content) : this(name, name, csvFile, Content)
        {

        }

        public void Play()
        {
            MediaPlayer.Play(audio);
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}

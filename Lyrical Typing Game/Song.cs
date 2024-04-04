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

        public Queue<(string, float)> Lyrics { get; } = new Queue<(string, float)>();

        public int Count { get; private set; }

        private Microsoft.Xna.Framework.Media.Song audio;

        public Song(string name, string csvFile, ContentManager Content) 
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
                        float timeStampSeconds;
                        if (float.TryParse(field, out timeStampSeconds)) {
                            Lyrics.Enqueue((currentLyric, timeStampSeconds));
                        } else
                        {
                            currentLyric = field;
                            if (!currentLyric.Equals(string.Empty)) Count++;
                        }
                    }
                }
            }

            audio = Content.Load<Microsoft.Xna.Framework.Media.Song>("Travel Light");
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

using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using NVorbis.Ogg;
using NVorbis;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using CsvHelper.Configuration;

namespace Lyrical_Typing_Game
{
    public class Song
    {
        public string Name { get; }
        
        /// <summary>
        /// Stores lyrics as string with timestamp of when the lyric is no longer shown
        /// </summary>
        public Queue<Lyric> LyricsQueue { get; private set; }

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

            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ",",
                ShouldQuote = (args) => true,
                Escape = '\"',
                InjectionOptions = InjectionOptions.Escape,
                Mode = CsvHelper.CsvMode.Escape,
            };

            List<Lyric> lyricsList = CsvCrud<Lyric>.Read(csvFile, config);

            LyricsQueue = new Queue<Lyric>(lyricsList);

            LyricsCount = lyricsList.FindAll(x => !x.Words.Equals(string.Empty)).Count;

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

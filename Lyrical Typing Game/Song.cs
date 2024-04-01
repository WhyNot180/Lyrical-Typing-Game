using Microsoft.VisualBasic.FileIO;
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

        public Queue<(string, float)> Lyrics { get; } = new Queue<(string, float)>();

        

        public Song(string name, string csvFile) 
        {
            Name = name;

            using (TextFieldParser parser = new TextFieldParser(csvFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
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
                        }
                    }
                }
            }
        }
    }
}

// File: Helpers/WordParserHelper.cs
// No changes needed as the code works the same with a class
using System.Text.RegularExpressions;

using Makara.Data;
using Makara.Models;

namespace Makara.Helpers
{
    public static class WordParserHelper
    {
        /// <summary>
        /// Parses the provided text into words (only Unicode letters are kept)
        /// and inserts them into the provided WordPickDatabase.
        /// </summary>
        /// <param name="text">The long text to parse.</param>
        /// <param name="database">The target database instance. If null, a new instance is created.</param>
        public static void ParseAndAddWords(string text, WordPickDatabase? database = null)
        {
            if(string.IsNullOrWhiteSpace(text))
                return;

            // Regex to match sequences of Unicode letters.
            var matches = Regex.Matches(text, @"\p{L}+");
            database ??= new WordPickDatabase();

            foreach(Match match in matches)
            {
                string wordStr = match.Value;
                // Create a WordModel using the parsed word. Status defaults to Unspecified.
                var wordModel = new WordModel { Word = wordStr };
                // Insert the word into the database.
                database.InsertWord(wordModel);
            }
        }
    }
}
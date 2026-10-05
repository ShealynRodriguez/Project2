using System;

namespace CodeBattle.Core
{
    /// <summary>One row in challenges.json. Strings so non-programmers can edit the file.</summary>
    [Serializable]
    public class ChallengeEntry
    {
        public string language;     // "Python" or "JavaScript"
        public string difficulty;   // "Easy", "Medium", or "Hard"
        public string prompt;       // code with a ___ blank
        public string[] answers;    // every accepted answer
    }

    [Serializable]
    public class ChallengeFile
    {
        public ChallengeEntry[] challenges;
    }

    public class Challenge
    {
        public string Prompt;
        public string[] Answers;
        public Difficulty Difficulty;
        public CodeLanguage Language;

        public bool IsCorrect(string input)
        {
            if (input == null) return false;
            string given = Normalize(input);
            foreach (string a in Answers)
            {
                if (Normalize(a) == given) return true;
            }
            return false;
        }

        /// <summary>First letter and length of the first accepted answer.</summary>
        public string HintText
        {
            get
            {
                string a = Answers[0];
                return "Starts with '" + a[0] + "', " + a.Length + " characters";
            }
        }

        static string Normalize(string s)
        {
            return s.Trim().ToLowerInvariant();
        }
    }
}

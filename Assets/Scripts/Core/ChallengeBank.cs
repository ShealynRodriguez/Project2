using System;
using System.Collections.Generic;

namespace CodeBattle.Core
{
    public class ChallengeBank
    {
        readonly List<Challenge> all = new List<Challenge>();

        public int Count { get { return all.Count; } }

        public ChallengeBank(ChallengeFile file)
        {
            if (file == null || file.challenges == null)
                throw new ArgumentException("Challenge file is empty or could not be read.");

            foreach (ChallengeEntry e in file.challenges)
            {
                all.Add(new Challenge
                {
                    Prompt = e.prompt,
                    Answers = e.answers,
                    Difficulty = (Difficulty)Enum.Parse(typeof(Difficulty), e.difficulty, true),
                    Language = (CodeLanguage)Enum.Parse(typeof(CodeLanguage), e.language, true),
                });
            }
        }

        public Challenge Pick(CodeLanguage language, Difficulty difficulty, Random rng)
        {
            var pool = new List<Challenge>();
            foreach (Challenge c in all)
            {
                if (c.Language == language && c.Difficulty == difficulty) pool.Add(c);
            }
            if (pool.Count == 0)
                throw new InvalidOperationException("No " + difficulty + " challenges for " + language + ". Add some to challenges.json.");
            return pool[rng.Next(pool.Count)];
        }
    }
}

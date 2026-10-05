using System.Collections.Generic;
using CodeBattle.Core;

namespace CodeBattle
{
    /// <summary>
    /// Everything that has to survive between scenes: chosen language, caught team,
    /// player level, and which fight we are on. Menus and fights both read from here.
    /// </summary>
    public static class GameSession
    {
        public static CodeLanguage Language;
        public static List<Creature> Team = new List<Creature>();
        public static PlayerProgress Player = new PlayerProgress();
        public static LevelDefinition[] Levels = LevelDefinition.CreateDefaults();
        public static int LevelIndex;
        public static ChallengeBank Bank;

        public static LevelDefinition CurrentLevel { get { return Levels[LevelIndex]; } }
        public static bool IsLastLevel { get { return LevelIndex >= Levels.Length - 1; } }

        /// <summary>Call this from the menu once the player has picked a language and starter.</summary>
        public static void StartNewGame(CodeLanguage language, Creature starter)
        {
            Language = language;
            Team = new List<Creature> { starter };
            Player = new PlayerProgress();
            Levels = LevelDefinition.CreateDefaults();
            LevelIndex = 0;
            if (Bank == null) Bank = ChallengeLoader.Load();
        }

        /// <summary>Call after a won fight. Returns false if that was the final fight.</summary>
        public static bool AdvanceLevel()
        {
            if (IsLastLevel) return false;
            LevelIndex++;
            return true;
        }
    }
}

namespace CodeBattle.Core
{
    /// <summary>
    /// Every balance number in one place. Tweak these in team meetings.
    /// </summary>
    public static class GameRules
    {
        public const float CatchWindow = 0.30f;        // can throw a ball at <= 30% enemy HP
        public const int StartingMaxHp = 50;
        public const int HpPerLevel = 10;
        public const int XpPerLevelMultiplier = 30;    // XP needed = level * 30
        public const int CatchXp = 20;
        public const int HintsPerFight = 1;
        public const float HintDamageMultiplier = 0.5f;

        public static int BaseDamage(Difficulty d)
        {
            switch (d)
            {
                case Difficulty.Easy: return 10;
                case Difficulty.Medium: return 20;
                default: return 30;
            }
        }

        public static int XpFor(Difficulty d)
        {
            switch (d)
            {
                case Difficulty.Easy: return 5;
                case Difficulty.Medium: return 10;
                default: return 15;
            }
        }
    }
}

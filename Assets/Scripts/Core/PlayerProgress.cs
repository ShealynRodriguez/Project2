namespace CodeBattle.Core
{
    public class PlayerProgress
    {
        public int Level = 1;
        public int Xp;
        public int MaxHp = GameRules.StartingMaxHp;
        public int Hp = GameRules.StartingMaxHp;

        public int XpToNextLevel { get { return Level * GameRules.XpPerLevelMultiplier; } }

        /// <summary>Adds XP and returns how many levels were gained.</summary>
        public int GainXp(int amount)
        {
            Xp += amount;
            int gained = 0;
            while (Xp >= XpToNextLevel)
            {
                Xp -= XpToNextLevel;
                Level++;
                MaxHp += GameRules.HpPerLevel;
                gained++;
            }
            return gained;
        }

        public void FullHeal() { Hp = MaxHp; }
    }
}

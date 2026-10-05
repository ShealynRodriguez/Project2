namespace CodeBattle.Core
{
    public class LevelDefinition
    {
        public string Name;
        public Creature Enemy;
        public int MaxHp;
        public int EnemyDamage;   // damage to the player on a wrong answer
        public bool IsBoss;

        public LevelDefinition(string name, Creature enemy, int maxHp, int enemyDamage, bool isBoss)
        {
            Name = name;
            Enemy = enemy;
            MaxHp = maxHp;
            EnemyDamage = enemyDamage;
            IsBoss = isBoss;
        }

        /// <summary>Three mini fights, then the boss.</summary>
        public static LevelDefinition[] CreateDefaults()
        {
            return new[]
            {
                new LevelDefinition("Mini fight 1", new Creature("Cinderbat", PokeType.Fire), 30, 10, false),
                new LevelDefinition("Mini fight 2", new Creature("Dripfin", PokeType.Water), 45, 12, false),
                new LevelDefinition("Mini fight 3", new Creature("Mossback", PokeType.Grass), 60, 15, false),
                new LevelDefinition("Boss fight", new Creature("Chimera King", PokeType.Fire), 120, 20, true),
            };
        }
    }
}

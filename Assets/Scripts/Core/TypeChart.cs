namespace CodeBattle.Core
{
    /// <summary>Fire beats Grass, Grass beats Water, Water beats Fire.</summary>
    public static class TypeChart
    {
        public const float SuperEffective = 1.5f;
        public const float NotVeryEffective = 0.5f;

        public static PokeType Beats(PokeType t)
        {
            switch (t)
            {
                case PokeType.Fire: return PokeType.Grass;
                case PokeType.Grass: return PokeType.Water;
                default: return PokeType.Fire;
            }
        }

        public static float Multiplier(PokeType attacker, PokeType defender)
        {
            if (Beats(attacker) == defender) return SuperEffective;
            if (Beats(defender) == attacker) return NotVeryEffective;
            return 1f;
        }
    }
}

namespace CodeBattle.Core
{
    public class Creature
    {
        public string Name;
        public PokeType Type;

        public Creature(string name, PokeType type)
        {
            Name = name;
            Type = type;
        }
    }

    public static class Roster
    {
        public static Creature[] CreateStarters()
        {
            return new[]
            {
                new Creature("Emberpup", PokeType.Fire),
                new Creature("Splashling", PokeType.Water),
                new Creature("Leafkit", PokeType.Grass),
            };
        }
    }
}

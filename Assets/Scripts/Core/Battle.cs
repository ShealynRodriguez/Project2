using System;

namespace CodeBattle.Core
{
    public enum BattleState { ChoosingMove, AwaitingAnswer, AwaitingCatchAnswer, Won, Lost }

    public struct AttackResult
    {
        public bool Correct;
        public int Damage;          // damage dealt to the enemy
        public float Multiplier;    // type effectiveness (1.5, 1, or 0.5)
        public int DamageTaken;     // damage the player took (wrong answer)
        public int LevelsGained;
        public BattleState NewState;
    }

    public struct CatchResult
    {
        public bool Caught;
        public int DamageTaken;
        public int LevelsGained;
        public BattleState NewState;
    }

    /// <summary>
    /// One fight. Pure C#, no Unity code, so it can be tested without opening the editor.
    /// Flow: StartAttack -> (UseHint) -> SubmitAnswer, or StartCatch -> SubmitCatch.
    /// </summary>
    public class Battle
    {
        static readonly PokeType[] BossPhases = { PokeType.Fire, PokeType.Water, PokeType.Grass };

        readonly ChallengeBank bank;
        readonly CodeLanguage language;
        readonly Random rng;

        Challenge current;
        Creature attacker;
        Difficulty currentDifficulty;
        bool hintUsed;

        public LevelDefinition Level { get; private set; }
        public PlayerProgress Player { get; private set; }
        public int EnemyHp { get; private set; }
        public int HintsLeft { get; private set; }
        public BattleState State { get; private set; }
        public Creature Caught { get; private set; }

        public Battle(LevelDefinition level, PlayerProgress player, ChallengeBank bank,
                      CodeLanguage language, Random rng = null)
        {
            Level = level;
            Player = player;
            this.bank = bank;
            this.language = language;
            this.rng = rng ?? new Random();

            Player.FullHeal();
            EnemyHp = level.MaxHp;
            HintsLeft = GameRules.HintsPerFight;
            State = BattleState.ChoosingMove;
        }

        /// <summary>The boss changes type as it loses HP. Normal enemies never change.</summary>
        public PokeType EnemyType
        {
            get
            {
                if (!Level.IsBoss) return Level.Enemy.Type;
                int lost = Level.MaxHp - EnemyHp;
                int phase = Math.Min(2, lost * 3 / Level.MaxHp);
                return BossPhases[phase];
            }
        }

        public bool CanThrowBall
        {
            get
            {
                return State == BattleState.ChoosingMove
                    && !Level.IsBoss
                    && EnemyHp <= Level.MaxHp * GameRules.CatchWindow;
            }
        }

        public Challenge StartAttack(Creature attackingCreature, Difficulty difficulty)
        {
            if (State != BattleState.ChoosingMove)
                throw new InvalidOperationException("Not ready for a new attack.");
            attacker = attackingCreature;
            currentDifficulty = difficulty;
            hintUsed = false;
            current = bank.Pick(language, difficulty, rng);
            State = BattleState.AwaitingAnswer;
            return current;
        }

        /// <summary>Returns the hint text, or null if no hint is available.</summary>
        public string UseHint()
        {
            bool answering = State == BattleState.AwaitingAnswer || State == BattleState.AwaitingCatchAnswer;
            if (!answering || HintsLeft <= 0 || hintUsed) return null;
            HintsLeft--;
            hintUsed = true;
            return current.HintText;
        }

        public AttackResult SubmitAnswer(string input)
        {
            if (State != BattleState.AwaitingAnswer)
                throw new InvalidOperationException("No attack challenge is active.");

            var result = new AttackResult();
            if (current.IsCorrect(input))
            {
                result.Correct = true;
                result.Multiplier = TypeChart.Multiplier(attacker.Type, EnemyType);
                float dmg = GameRules.BaseDamage(currentDifficulty) * result.Multiplier;
                if (hintUsed) dmg *= GameRules.HintDamageMultiplier;
                result.Damage = (int)dmg;

                EnemyHp -= result.Damage;
                if (!Level.IsBoss && EnemyHp < 1) EnemyHp = 1;   // mini-fight enemies can only be caught
                result.LevelsGained = Player.GainXp(GameRules.XpFor(currentDifficulty));

                if (Level.IsBoss && EnemyHp <= 0)
                {
                    EnemyHp = 0;
                    State = BattleState.Won;
                }
                else
                {
                    State = BattleState.ChoosingMove;
                }
            }
            else
            {
                result.DamageTaken = TakeHit();
            }
            result.NewState = State;
            return result;
        }

        public Challenge StartCatch()
        {
            if (!CanThrowBall)
                throw new InvalidOperationException("The enemy is not weak enough to catch yet.");
            hintUsed = false;
            current = bank.Pick(language, Difficulty.Medium, rng);
            State = BattleState.AwaitingCatchAnswer;
            return current;
        }

        public CatchResult SubmitCatch(string input)
        {
            if (State != BattleState.AwaitingCatchAnswer)
                throw new InvalidOperationException("No catch challenge is active.");

            var result = new CatchResult();
            if (current.IsCorrect(input))
            {
                result.Caught = true;
                Caught = Level.Enemy;
                result.LevelsGained = Player.GainXp(GameRules.CatchXp);
                State = BattleState.Won;
            }
            else
            {
                result.DamageTaken = TakeHit();
            }
            result.NewState = State;
            return result;
        }

        int TakeHit()
        {
            int d = Level.EnemyDamage;
            Player.Hp -= d;
            if (Player.Hp <= 0)
            {
                Player.Hp = 0;
                State = BattleState.Lost;
            }
            else
            {
                State = BattleState.ChoosingMove;
            }
            return d;
        }
    }
}

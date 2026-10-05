using System;
using UnityEngine;
using CodeBattle.Core;

namespace CodeBattle
{
    /// <summary>
    /// Put this on an object in the battle scene. It runs a Battle and raises events.
    /// The Systems/UI programmer subscribes to the events to update health bars, text,
    /// win/lose screens, etc. The Gameplay programmer owns the rules in Scripts/Core.
    ///
    /// UI buttons call: PickMove, ThrowBall, UseHint, SubmitAnswer, RestartFight.
    /// </summary>
    public class BattleController : MonoBehaviour
    {
        public event Action<Battle> FightStarted;
        public event Action<Challenge> ChallengeShown;
        public event Action<string> HintShown;
        public event Action<AttackResult> AttackResolved;
        public event Action<CatchResult> CatchResolved;
        public event Action<Battle> FightEnded;   // check Battle.State for Won or Lost

        public Battle Current { get; private set; }

        public void BeginFight()
        {
            Current = new Battle(GameSession.CurrentLevel, GameSession.Player,
                                 GameSession.Bank, GameSession.Language);
            if (FightStarted != null) FightStarted(Current);
        }

        /// <summary>difficultyIndex: 0 = Easy, 1 = Medium, 2 = Hard.</summary>
        public void PickMove(int teamIndex, int difficultyIndex)
        {
            Creature attacker = GameSession.Team[teamIndex];
            Challenge challenge = Current.StartAttack(attacker, (Difficulty)difficultyIndex);
            if (ChallengeShown != null) ChallengeShown(challenge);
        }

        public void ThrowBall()
        {
            if (!Current.CanThrowBall) return;
            Challenge challenge = Current.StartCatch();
            if (ChallengeShown != null) ChallengeShown(challenge);
        }

        public void UseHint()
        {
            string hint = Current.UseHint();
            if (hint != null && HintShown != null) HintShown(hint);
        }

        public void SubmitAnswer(string answer)
        {
            if (Current.State == BattleState.AwaitingCatchAnswer)
            {
                CatchResult result = Current.SubmitCatch(answer);
                if (result.Caught) GameSession.Team.Add(Current.Caught);
                if (CatchResolved != null) CatchResolved(result);
            }
            else if (Current.State == BattleState.AwaitingAnswer)
            {
                AttackResult result = Current.SubmitAnswer(answer);
                if (AttackResolved != null) AttackResolved(result);
            }

            if ((Current.State == BattleState.Won || Current.State == BattleState.Lost)
                && FightEnded != null)
            {
                FightEnded(Current);
            }
        }

        /// <summary>Losing a fight means retrying the same fight (the checkpoint).</summary>
        public void RestartFight()
        {
            BeginFight();
        }
    }
}

# How the code is organized

The game rules are written as plain C# with **no Unity code**, so they can be tested without opening the editor. A thin Unity layer sits on top and connects them to the scene.

```
Assets/Scripts/
  Core/    The rules (Gameplay Programmer owns this)
  Game/    Unity glue: scene controllers and saved game state
Assets/Resources/challenges.json    All code questions (anyone can edit)
```

## Core (rules, no Unity)
| File | What it does |
|---|---|
| `GameRules.cs` | Every balance number: damage per tier, XP, HP per level, catch window |
| `TypeChart.cs` | Fire beats grass, grass beats water, water beats fire (1.5x / 0.5x) |
| `Battle.cs` | One fight: attack, hint, catch, boss phases, win or lose |
| `Challenge.cs`, `ChallengeBank.cs` | A code question and the pool the game picks from |
| `PlayerProgress.cs` | Level, XP, HP |
| `LevelDefinition.cs` | The 3 mini fights and the boss |
| `Creature.cs` | A Pokemon (name and type) and the starter list |

## Game (Unity glue)
| File | What it does |
|---|---|
| `GameSession.cs` | Remembers language, team, player level, and fight number between scenes |
| `BattleController.cs` | Runs a `Battle` in a scene and raises events |
| `ChallengeLoader.cs` | Loads `challenges.json` |

## How the UI plugs in (Systems / UI Programmer)
`BattleController` raises events. Subscribe to them to update the screen, and call its methods from buttons:

| Event | Use it to |
|---|---|
| `FightStarted` | Set up health bars and the enemy sprite |
| `ChallengeShown` | Show the code question and input box |
| `HintShown` | Show the hint text |
| `AttackResolved` | Show damage, "Super effective!", level-up, update bars |
| `CatchResolved` | Show caught or "it broke free" |
| `FightEnded` | Show the win or lose screen (check `Battle.State`) |

| Button calls | When |
|---|---|
| `PickMove(teamIndex, difficultyIndex)` | Player picks a Pokemon and a move tier (0 easy, 1 medium, 2 hard) |
| `SubmitAnswer(text)` | Player submits their code |
| `ThrowBall()` | Enemy is weak enough (`Battle.CanThrowBall`) |
| `UseHint()` | Player asks for a hint |
| `RestartFight()` | Player lost and retries |

A menu scene calls `GameSession.StartNewGame(language, starter)` and then loads the battle scene. After a win, call `GameSession.AdvanceLevel()`.

## Adding code questions (no programming needed)
Open `Assets/Resources/challenges.json` and copy a block:
```json
{
  "language": "Python",
  "difficulty": "Easy",
  "prompt": "for i in ___(3):   # loop 3 times",
  "answers": ["range"]
}
```
`language` is `Python` or `JavaScript`. `difficulty` is `Easy`, `Medium`, or `Hard`. `answers` lists every accepted answer (matching ignores capitals and extra spaces). Each language and difficulty needs at least one question, and several to avoid repeats.

## Tuning the game
Change numbers in `GameRules.cs` (damage, XP, catch window) and `LevelDefinition.cs` (enemy HP and damage).

## Testing
The Core has been compiled and checked outside Unity (type chart, damage, hints, catch window, boss phases, XP, win and lose). The `Game/` scripts need the Unity editor and haven't been run yet, so expect small fixes the first time you hit Play.

The original Python prototype in `prototype/` is the reference for how the rules should feel.

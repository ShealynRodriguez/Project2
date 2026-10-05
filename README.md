# Pokemon Code Battle

A Pokemon-style, type-based battle game where you fight by writing code. Solve the coding challenge and your attack lands. Get it wrong and it misses. Built in **Unity 6.3 LTS**.

**Team meeting:** Tuesdays at 8:00 PM

## How it works
1. Pick a programming language before the game starts.
2. Fight three mini battles. Catch each Pokemon to win it.
3. Take your caught team into one big boss fight.
4. Every attack is a code challenge (easy, medium, or hard). Harder code does more damage, wrong code does none.

Full notes and open questions: [`docs/meeting-notes.md`](docs/meeting-notes.md)

## Repo layout
```
Assets/
  Scripts/Core/     Game rules (plain C#, tested outside Unity)
  Scripts/Game/     Unity glue: BattleController, GameSession
  Resources/        challenges.json (all the code questions)
  Sprites/          Fire, water, grass placeholder sprites
  Scenes/ Prefabs/ UI/ Audio/ Tilesets/    Ready for the team to fill
docs/               Meeting notes, role guides, Unity setup, architecture
prototype/          Original Python terminal prototype (design reference)
CONTRIBUTING.md     Git workflow for the team
```
Unity's `Packages/` and `ProjectSettings/` get added during setup (see below).

## Start here
1. **Setup:** [`docs/unity-setup.md`](docs/unity-setup.md). Same Unity version for everyone, and one person does the first-time project setup.
2. **Git:** [`CONTRIBUTING.md`](CONTRIBUTING.md). Branch, commit small, pull request.
3. **Code:** [`docs/architecture.md`](docs/architecture.md). Where things live and how the UI hooks in.

## Team roles
- [Gameplay Programmer](docs/roles/gameplay-programmer.md)
- [Systems / UI Programmer](docs/roles/systems-ui-programmer.md)
- [Art / Audio / Level Designer](docs/roles/art-audio-level-designer.md)

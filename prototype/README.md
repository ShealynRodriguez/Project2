# Terminal prototype (Python)

A playable text version of the battle and leveling rules, used to test the design before building in Unity. The Unity version of these rules lives in `Assets/Scripts/Core` (see `docs/architecture.md`).

```
python3 code_battle_prototype.py
```

No installs needed (Python 3). Balance numbers, the type chart, and all code challenges are at the top of the file.

Covers: language pick, starter pick, 3 mini fights (catch to clear), boss fight, type chart, XP and levels, hints, and a catch window at 30% enemy HP.
Not decided yet: see the open questions in `docs/meeting-notes.md`.

Note: `Assets/Resources/challenges.json` was generated from this file's questions. If you add questions, add them to the JSON (that's what the Unity game reads).

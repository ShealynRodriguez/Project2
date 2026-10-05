# Unity setup (do this once, then everyone follows the short version)

**Everyone must use the same Unity version: Unity 6.3 LTS.** Mixed versions rewrite project files and cause constant conflicts. Install it through Unity Hub and pick the exact same 6.3.x build, and don't upgrade mid-project.

## Step 1: one person creates the Unity project files (Gameplay Programmer)
The repo already has the `Assets/` folders, scripts, and sprites. Unity itself has to generate `Packages/` and `ProjectSettings/`.

1. Clone the repo.
2. In Unity Hub: **New project** → **Universal 2D** template → name it `pokemon-code-battle` → save it somewhere **outside** the repo (like your Desktop).
3. Open it once, let it finish importing, then close Unity.
4. Copy these from the new project into the repo root:
   - `Packages/`
   - `ProjectSettings/`
   - any starter folders Unity put inside `Assets/` that we don't already have (for example `Settings/`)
   Don't overwrite our existing folders (`Scripts`, `Sprites`, `Resources`, and so on).
5. In Unity Hub: **Add** → **Add project from disk** → pick the repo folder. Open it.
6. Check **Edit → Project Settings → Editor**:
   - Version Control Mode: **Visible Meta Files**
   - Asset Serialization: **Force Text**
7. Unity will create a `.meta` file next to every file in `Assets/`. **Commit all of them.** Then push.

## Step 2: everyone else
1. `git clone <repo-url>`
2. Unity Hub → **Add** → **Add project from disk** → pick the cloned folder.
3. First open takes a few minutes (it builds a local `Library/` folder, which is not committed).

## Pixel art import settings
Select the sprites in `Assets/Sprites` and set these in the Inspector, then click **Apply**, or the art will look blurry:
- Texture Type: **Sprite (2D and UI)**
- Pixels Per Unit: **32** (our sprites are 32x32)
- Filter Mode: **Point (no filter)**
- Compression: **None**

Use the 32px files. The `_16x` files are only big previews.

## Rules that save the team from Unity pain
- **Always commit `.meta` files with their asset.** If a `.meta` goes missing, references break for everyone.
- **One person edits a given scene at a time.** Scene merge conflicts are miserable. Say in Discord "I'm in the Battle scene".
- **Build things as prefabs** (a health bar, an enemy, a menu panel) so people can work in different files without colliding.
- **Never commit** `Library/`, `Temp/`, `Logs/`, or `Builds/`. The `.gitignore` already blocks them.
- **Make an export build early** (File → Build Profiles) and run it on a different computer.

## Suggested scene list
| Scene | Owner |
|---|---|
| `MainMenu` (language and starter pick) | Systems / UI |
| `Battle` (fights, health bars, code box) | Gameplay + Systems / UI |
| `Victory`, `Defeat` | Systems / UI |
| `Levels` (overworld or backgrounds) | Art / Audio / Level |

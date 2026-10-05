# Unity setup

everyone needs the SAME unity version or the project files get messed up

unity version we're using: ____________

## first person only (making the project)
1. clone the repo
2. unity hub > new project > Universal 2D, save it somewhere outside the repo
3. open it once then close it
4. copy the Packages and ProjectSettings folders into the repo
5. unity hub > add project from disk > pick the repo folder
6. Edit > Project Settings > Editor: set Version Control to Visible Meta Files and Asset Serialization to Force Text
7. commit everything (including .meta files) and push

## everyone else
1. clone the repo
2. unity hub > add project from disk > pick the folder
3. first time opening takes a while

## sprite settings so the pixel art isnt blurry
- Texture Type: Sprite (2D and UI)
- Pixels Per Unit: 32
- Filter Mode: Point (no filter)
- Compression: None

use the 32px sprites, the 16x ones are just big previews

## scenes
| scene | who |
|---|---|
| MainMenu | |
| Battle | |
| Win / Lose | |
| | |

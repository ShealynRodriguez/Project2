# Git workflow

Short version: **never work directly on `main`**. Make a branch, commit small, open a pull request.

## One-time setup
```
git clone <repo-url>
cd pokemon-code-battle
git config user.name "Your Name"
git config user.email "you@example.com"
```

## Every time you work
```
git checkout main
git pull                                     # get the latest from the team
git checkout -b yourname/short-description   # e.g. sam/main-menu

# ...make changes...

git add <files>
git commit -m "Add main menu screen"
git push -u origin yourname/short-description
```
Then open a **pull request** on GitHub and ask a teammate to look at it before merging.

## Rules of thumb
- Commit small and often. One idea per commit.
- Write commit messages that say what changed ("Add fire sprite"), not "stuff".
- Pull before you start working each day.
- Stuck on a merge conflict? Ask in the Discord before deleting anything.
- Don't commit build output, editor folders, or big files you can regenerate. `.gitignore` handles the common ones.
- Check the license before adding any asset you didn't make.

## Unity-specific rules
- **Commit `.meta` files together with the asset they belong to.** Never delete or ignore them.
- **Everyone uses the same Unity version** (6.3 LTS). Don't upgrade the project.
- **One person per scene at a time.** Tell the Discord before you open a scene. Prefer prefabs so people don't edit the same file.
- Before committing, check `git status` for surprises. If you see `Library/` or `Temp/`, stop and ask.
- Full setup steps: `docs/unity-setup.md`.

## Branch names
`name/what-you-are-doing`, for example `sam/main-menu`, `alex/forest-tileset`, `jo/catch-logic`

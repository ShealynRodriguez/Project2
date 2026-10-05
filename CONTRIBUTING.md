# how we're using git

dont push straight to main pls. make a branch and do a pull request

## first time
```
git clone https://github.com/ShealynRodriguez/Project2.git
git config user.name "your name"
git config user.email "your github email"
```

## every time
```
git checkout main
git pull
git checkout -b yourname/what-youre-doing

(do your stuff)

git add .
git commit -m "what you did"
git push -u origin yourname/what-youre-doing
```
then go on github and open a pull request, have someone look at it before merging

## unity stuff
- always commit the .meta files with whatever file they go with
- everyone use the same unity version
- only one person in a scene at a time, say in the group chat if youre editing one
- if you see Library/ or Temp/ in git status dont commit it

## branch name examples
- shealyn/main-menu
- yourname/forest-tileset
- yourname/catch-logic

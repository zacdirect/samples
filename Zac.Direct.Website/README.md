# Relocated

I decided the source and build repository for my personal site might be too difficult to keep as a generic sample.  It has been [moved here](https://github.com/zacdirect/zac.direct).

## Moving a folder out of a mono repo while keeping full history

```bash
#Make a new branch that contains only the history of the folder you want to move.
git subtree split -P Zac.Direct.Website -b split-branch
#Leave the repo.
cd ..
#Make your new destination repo and initialize it
mkdir zac.direct
git init
#Create a blank remote repo
#Then add it and push like normal
git remote add origin https://github.com/zacdirect/zac.direct.git
git branch -M main
git push -u origin main
```

Keeping history on your git repos is always possible.  In certain circumstances such as the opposite of this use-case, where you want to move a series of disconnected repos into a single mono repo it's less straightforward.  Requiring a combination of `--allow-unrelated-histories`, potentially temporarily removing branch restrictions, and force pushes.

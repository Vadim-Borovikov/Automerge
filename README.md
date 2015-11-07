# Automerger
## Descriprion:
Merges base file's changes from two sources.
## Merge rules:
1. Two identical changes don't yield a conflict
2. Two different additions before same line yield a confilct
3. In other cases two changes without collision don't yield a conflict
4. Addition vs removal yield a replacement
5. In other cases two changes with collision yield a conflict

## Created with
* Microsoft Visual Studio 2015
* C#
* Windows Forms

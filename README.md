# Project ICS 2021 
__Topic:__ Festival Management Application

__Team Members:__
- Daniel Peřina (xperin12)
- Oleksandr Prokofiev (xproko40)
- Magdaléna Bellayová (xbella01)
- Dalibor Beneš (xbenes56)
- Marek Kafoněk (xkafon01)

__Deadlines:__
1. __14.03.__ – Object design (20b)
2. __11.04.__ – Entity Framework, Repository, Tests (30b)
3. __04.05__ – Finalization and defense (50b)

# Strategy
__Meetings:__ Friday 13.00

Branch strategy: 'GitFlow' (shown in lecture 01)
- Main branch for submissions
- Develop branch for development (shocking)
- For each issue create new branch

Work strategy: 'Scrum'
- Detailed description in discord
- Commits naming: short and concise, one commit for each file
	- Create \[File\]
	- Delete \[File\]
	- Fixup \[File\] \[Opt. info\]
	- Update \[File\] \[Opt. Info\]

# Conventions
__Language:__ All in English

## Casing
- Classes: PascalCase
	- Private fields: _camelCase, noun
	- Properties: PascalCase, noun
	- Methods: PascalCase, verb
	- Local variables: camelCase
- Interfaces: IPascalCase
- Constants: CAPITAL_LETTERS

## Structure
- Every class (struct, enum, record...) in its own file
	- Try to keep files small (it should fit on the screen)
	- Try to keep methods small (under 10 lines if possible)
- Use tab for indentation
- All braces will be on their own line

# Links
- [Course repository](https://github.com/nesfit/ICS)
- [Project assignment](https://github.com/nesfit/ICS/tree/master/Project)
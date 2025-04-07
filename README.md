# TheLabyrinth

**The Labyrinth** is a puzzle game where you have to get to the center of the labyrinth in a limited time.
*In the future*, there may appear various collectible bonuses, annoying enemies, new game modes and new supported platfomrs.

But above all, this game is an example of using useful patterns and practices:
- Most of the code that does not depend on Unity components is decomposed into concise interfaces and passed to the necessary places using **dependency injection** (by Zenject), which gives more *flexibility* to the project and more *reusable modules*
- Work with the UI is organized using the **MVP** pattern, which allows you to quickly find errors in the UI and develop the backend independently of the frontend
- The game flow is organized using a **finite state machine**, which helps to *better decompose* the game logic and add new states faster
- The code is split into **different assemblies**, allowing for *faster development*, *easier testing*, and faster bug finding

## Gameplay

![Gameplay](Gameplay.gif)

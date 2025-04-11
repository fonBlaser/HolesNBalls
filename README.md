# HolesNBalls
The 'Holes and Balls' game with a console application and simple solver (breadth-first search).

## Game rules
The game is played on an N × N board filled with uniquely numbered Balls and Holes placed at specific spots.
### How to Play
- You can move the entire board in one of four directions: Up, Down, Left, or Right.
- When you move the board, all Balls slide in that direction until they hit something—either a wall or another Ball.
- The goal is to get each Ball into the Hole with the matching number.
- If a Ball goes into the wrong Hole, you lose the game.

## Solution structure

- [Source](./Source)
  - [HolesNBalls](./Source/HolesNBalls) - Main library with board, solver, and validations
  - [HolesNBalls.Cli](./Source/HolesNBalls.Cli) - Console application with instructions
- [Tests](./Tests)
  - [HolesNBalls.Tests.Unit](./Tests/HolesNBalls.Tests.Unit) - Unit tests for board, solver, and validations

## Main entities
- [Board](./Source/HolesNBalls/Board.cs) - Game board containing [Holes](./Source/HolesNBalls/Hole.cs) and [Balls](./Source/HolesNBalls/Ball.cs). Has `Move(Direction)` method. If the Ball meets the Hole, the [Drop](./Source/HolesNBalls/Drop.cs) is created, and Ball is removed.
- [BoardValidator](./Source/HolesNBalls/Validation/BoardValidator.cs) - Checks if the Board and objects in it are correct.
- [BfsSolver](./Source/HolesNBalls/Solving/BfsSolver.cs) - Breadth-first solver for game. Uses different [Modes](./Source/HolesNBalls/Solving/BfsSolutionMode.cs). Returns collection of suitable last [Turns](./Source/HolesNBalls/Solving/Turn.cs).

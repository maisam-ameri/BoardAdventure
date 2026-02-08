# BoardAdventure

**BoardAdventure** is a Unity-based prototype of a classic board game inspired by **Ludo** (also known as **Mench**).  
This project focuses on core gameplay mechanics and clean architectural separation, and is intended as a learning and portfolio project.

---

## Overview

BoardAdventure is a turn-based board game prototype built with Unity and C#.  
The main goal of the project is to implement and experiment with core board-game logic such as dice rolling, player turns, token movement, and game state management.

The project is intentionally kept at a **prototype level**, prioritizing clarity of logic and structure over visual polish.

---

## Why This Project?

This project was created to explore scalable architecture patterns for turn-based games in Unity, 
with a focus on decoupled systems, testability, and future multiplayer support.

---

## Gameplay

The gameplay loop includes:

- Turn-based player system  
- Dice roll mechanics  
- Token movement across a board  
- Basic win-condition handling  
- Simple UI feedback for player actions  

Players take turns rolling a dice and moving their pieces until one player completes the required path on the board.

---

## Project Structure

The project follows a typical Unity structure with clear separation of responsibilities:

    Assets/
    ├── Scenes/        # Game scenes
    ├── Scripts/       # Core gameplay logic
    ├── Prefabs/       # Reusable game objects
    ├── UI/            # User interface elements
    ProjectSettings/
    Packages/

This structure allows the project to remain modular and easy to extend.


---

## Architecture Overview

The project follows a **service-oriented and event-driven architecture** designed for a turn-based board game.

Game logic is separated into small, focused services that communicate through signals rather than direct references.  
All major systems depend on abstractions, allowing implementations to be easily replaced or extended.

### Core Principles

- Clear separation between gameplay logic, data flow and presentation
- Service-based architecture with well-defined responsibilities
- Decoupled communication using signal-like method flows
- Designed with scalability and multiplayer support in mind

### Services & Game Flow

The main game loop is handled by dedicated services such as:

- Match flow and turn management
- Dice rolling and movement calculation
- Pawn creation and board navigation
- Player state and rewards

Each service is injected and communicates through high-level events such as action start/end and turn switching.

### Pawn & Board System

Pawns are created via factories and managed by a dedicated pawn manager.  
Movement is handled asynchronously based on calculated paths over board nodes, keeping gameplay logic independent from visuals.

### Data Layer

The architecture supports two data sources:

- **Mock Data** – currently used for local gameplay and prototyping
- **Network Data** – planned and partially designed, but not fully implemented

This separation allows future multiplayer or online features without major refactoring.

### Design Goal

The overall goal of this architecture is to keep the codebase modular, testable and easy to evolve while maintaining a clear and readable gameplay flow.


---

## Data Handling

The project currently uses two different data approaches:

### Mock Data (Primary)

Most of the gameplay logic is driven by **mock data**.  
This allows the game to function independently of external services and makes it easier to test and iterate on core mechanics.

### Network Data (Incomplete)

There is partial implementation intended for **network-based data handling**, aimed at future multiplayer support.  
This part of the project is **not complete** and is included as an experimental foundation rather than a finished feature.

---

## Current State

- ✅ Core gameplay mechanics implemented  
- ✅ Clean and readable project structure  
- ⚠️ Network layer incomplete  
- ⚠️ Visuals and polish kept minimal  

This repository represents a functional prototype rather than a final product.

---

## Future Improvements

Possible future directions for this project include:

- Completing the network multiplayer layer  
- Adding AI-controlled players  
- Improving visuals and animations  
- Enhancing UI and feedback  
- Expanding game rules and variations  

---


## Author

Created by **Maisam Ameri**  
This project is part of a personal learning journey and portfolio.

⭐ If you find this project interesting or useful, consider giving it a star.




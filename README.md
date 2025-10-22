# Puzzled-Skeleton

Puzzled-Skeleton is a small C# WPF game where the player, a skeleton who was once human, must navigate through a series of dungeon chambers to find a way out. Along the journey, the skeleton discovers a photo from its past, which restores it to its human form. The game is built using C# with WPF for the interface and visuals.

// TODO: Preview image

## Features

// TODO: Features

## Getting Started

### Prerequisites

Ensure you have the following installed on your system:
- Runtime: .NET SDK 8.0
- Build tools: Visual Studio 2022

### Building

Build instructions for this project can be found in the [BUILDING.md](BUILDING.md) file. Supported platforms are:
- **Windows**: Visual Studio 2022

## TODO List

---
### Priorities
🟩 **Must have** – Game won’t work without it<br>
🟨 **Should have** – Very important, adds major value<br>
🟧 **Could have** – Nice-to-have, improves experience<br>
🟥 **Would have / Wont have** – Extra polish, not needed for gameplay<br>

---

- [x] 🟨 Rename repository

- [ ] Core Systems
  - [x] Renderer
    - [x] 🟩 Quad renderer
    - [x] 🟩 Textured quad
    - [x] 🟩 Texture coordinates
    - [x] 🟩 Make pixel space
    - [x] 🟩 Flip Y-axis
    - [x] 🟩 US22 Improve efficiency
    - [x] 🟩 Animation system
    - [x] 🟩 Flip sprites
  - [x] Audio
    - [x] 🟩 Play wav sounds
    - [x] 🟧 Stop creating/opening a new file object each time
    - [x] 🟩 Close audio on close window to prevent lag/screen tear
  - [x] Input
    - [x] 🟩 Live key readback (IsKeyPressed())
    - [x] 🟩 Live mouse readback (IsMouseButtonPressed())
    - [x] 🟩 Live mouse position readback (GetMousePosition())
  - [x] UI
    - [x] 🟩 Font caching
    - [x] 🟩 Draw text
    - [x] 🟩 Remove text class
  - [ ] Scene
    - [x] 🟩 Fix UICanvas blocking GameCanvas
    - [ ] 🟧 Allow for scene passthrough of control of objects (sounds)

- [ ] Core Game
  - [ ] UI
    - [ ] Global
      - [ ] 🟩 US21/US41 Simpele interface
    - [ ] Menus
      - [ ] 🟩 Main Menu
        - [ ] 🟩 "Press Any Key" Font selection
      - [ ] 🟨 Pause Menu
      - [ ] 🟥 Level Menu
      - [ ] 🟩 Win Menu + Score + Leaderboard
      - [ ] 🟩 Save (selection) Menu
      - [ ] 🟨 Level overlay
      - [ ] 🟥 Final win menu (showing achievements)
      - [ ] 🟥 Options menu (sound slider)
  - [ ] Levels
    - [x] Global
      - [x] 🟩 Level loading from disk
    - [ ] Design
      - [ ] 🟩 Level 1 (🟩 normal / 🟧extra hard)
      - [ ] 🟨 Level 2 (🟨 normal / 🟧extra hard)
      - [ ] 🟨 Level 3 (🟨 normal / 🟧extra hard)
      - [ ] 🟨 Level 4 (🟨 normal / 🟧extra hard)
      - [ ] 🟨 Level 5 (🟨 normal / 🟧extra hard)
      - [ ] 🟨 Level 6 (🟨 normal / 🟧extra hard)
      - [x] 🟧 Texture for skeleton
      - [ ] 🟧 Texture for background(s)
      - [ ] 🟧 Texture for objects
      - [x] 🟧 Texture for platform blocks
  - [ ] Mechanics
    - [x] 🟩 Gravity
    - [x] 🟩 Physics (gravity/collision)
    - [x] 🟩 Movement (WASD/Arrow keys)
    - [ ] Puzzle mechanics
      - [ ] 🟩 Time = score
      - [ ] 🟧 Movable bricks (on to moving platform?)
      - [ ] 🟧 UV Flashlight
    - [ ] Trap mechanics
      - [ ] 🟧 US23 Spike (respawn bottom level)
  - [ ] Audio
    - [x] 🟨 Main menu/save selection music (Start-up, loop)
    - [ ] 🟧 Level 1
    - [ ] 🟧 Level 2
    - [ ] 🟧 Level 3
    - [ ] 🟧 Level 4
    - [ ] 🟧 Level 5
    - [ ] 🟧 Level 6
    - [ ] 🟧 Win/finish menu
    - [ ] 🟥 Sound for beating your high score (SFX)
    - [ ] 🟥 Final win menu
    - [ ] 🟨 Jump SFX
    - [ ] 🟥 Moving brick
    - [ ] 🟧 Flashlight click
    - [ ] 🟧 Moving sound
    - [ ] 🟧 Collect key sound
    - [ ] 🟧 Open door with key sound
  - [ ] Easter eggs + Achievements
    - [ ] 🟥 25x piano note
    - [ ] 🟥 Mario (UV flashlight reveal)

## License

All rights reserved.

This project is provided for viewing purposes only.  
You may not copy, modify, distribute, or use any part of this code  
without explicit written permission from the authors.

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes.

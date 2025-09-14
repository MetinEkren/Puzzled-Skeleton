# Puzzled-Skeleton

Dungeon Escape is a small C# WPF game where the player, a skeleton who was once human, must navigate through a series of dungeon chambers to find a way out. Along the journey, the skeleton discovers a photo from its past, which restores it to its human form. The game is built using C# with WPF for the interface and visuals.

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
🟪 **Core** – Must exist (foundation of the game)<br>
🟥 **Critical** – Game won’t work without it<br>
🟧 **High** – Very important, adds major value<br>
🟨 **Medium** – Nice-to-have, improves experience<br>
🟩 **Low** – Extra polish, not needed for gameplay<br>

---

- [x] 🟧 Rename repository

- [ ] Core Systems
  - [ ] Renderer
    - [x] 🟪 Quad renderer
    - [x] 🟪 Textured quad
    - [x] 🟪 Texture coordinates
    - [ ] 🟪 Make pixel space
    - [ ] 🟪 Flip Y-axis
  - [ ] Audio
    - [ ] 🟩 Play mp3 sounds
    - [x] 🟪 Play wav sounds
    - [ ] 🟨 Stop creating/opening a new file object each time
  - [ ] Input
    - [ ] 🟪 Live key readback (IsKeyPressed())
    - [ ] 🟪 Live mouse readback (IsMouseButtonPressed())
    - [ ] 🟪 Live mouse position readback (GetMousePosition())

- [ ] Core Game
  - [ ] UI
    - [ ] 🟥 Main Menu
    - [ ] 🟧 Pause Menu
    - [ ] 🟨 Level Menu
    - [ ] 🟧 Win Menu + Score
    - [ ] 🟥 Save (selection) Menu
    - [ ] 🟧 Level overlay
    - [ ] 🟨 Leaderboard
    - [ ] 🟨 Final win menu (showing achievements)
  - [ ] Levels
    - [ ] Design
      - [ ] 🟥 Level 1 (🟥 normal / 🟨extra hard)
      - [ ] 🟥 Level 2 (🟥 normal / 🟨extra hard)
      - [ ] 🟥 Level 3 (🟥 normal / 🟨extra hard)
      - [ ] 🟥 Level 4 (🟥 normal / 🟨extra hard)
      - [ ] 🟥 Level 5 (🟥 normal / 🟨extra hard)
      - [ ] 🟥 Level 6 (🟥 normal / 🟨extra hard)
      - [ ] 🟥 Texture for skeleton
      - [ ] 🟧 Texture for background(s)
      - [ ] 🟥 Texture for objects
      - [ ] 🟥 Texture for platform blocks
  - [ ] Mechanics
    - [ ] 🟥 Gravity
    - [ ] 🟥 Physics (gravity/collision)
    - [ ] 🟥 Movement (WASD/Arrow keys)
    - [ ] Puzzle mechanics
      - [ ] 🟧 Time = score
      - [ ] 🟧 Movable bricks
      - [ ] 🟧 UV Flashlight
  - [ ] Audio
    - [ ] 🟨 // TODO: Someone outline everything needed.
  - [ ] Easter eggs + Achievements
    - [ ] 🟩 25x piano note
    - [ ] 🟩 Mario (UV flashlight reveal)

## License

This project is licensed under the Apache 2.0 License. See [LICENSE](LICENSE.txt) for details.

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes.

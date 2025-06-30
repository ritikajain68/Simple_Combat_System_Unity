# Simple_Combat_System_Unity

## Objective
To build a self-running, AI-controlled battle simulator where multiple characters spawn, seek enemies, attack, and continue until a single winner remains. This project simulates strategic combat behaviors and showcases modular AI logic with Unity’s built-in systems like NavMesh, Animator, and UI.

<br>

## Project Scope

This Unity system should simulate a fully automated battle between AI-controlled players. It should support:
1. Spawn points and character instantiation
2. Navigate characters toward opponents engaging in combat when within specific attack range.
3. Automatically re-target when an opponent dies or becomes invalid.
4. Aims to handle special case of detecting stuck agents and resolve using fallback strategies

<br>

## Design Overview

This project is a **modular, autonomous battle simulator** built in Unity. Multiple AI-controlled characters spawn on a battlefield, navigate using Unity's `NavMeshAgent`, and engage in ranged combat using projectile-based weapons until only one player remains. The system is built for clarity, modularity, and extensibility.

### Key System Modules

| Module | Description |
|--------|-------------|
| **PlayerSpawner** | Spawns all AI characters at valid spawn points, tracks the game state, and declares a winner. |
| **PlayerSetup** | Central AI controller for each character: finds targets, manages movement and attack timing, and tracks kill stats. |
| **PlayerWeapon / PlayerBullet** | Handles projectile-based shooting logic, including fire rate, range checking, raycast validation, and bullet instantiation. |
| **PlayerHealthBar** | Manages health values, damage intake, UI updates, and death detection. |
| **PlayerMovement** | Uses `NavMeshAgent` for pathfinding toward current targets, and manages movement animations. |
| **TopDownCameraController / CameraUIButton** | Provides top-down camera controls via user input or UI buttons. |

---

<br>

##  Approach

### 1. Modular, Reusable Architecture

Each component (e.g., movement, health, weapon) is its own script. This separation enables:
- **Easy testing**
- **Clear responsibilities**
- **Flexible scaling** (e.g., increase character count with no code changes)

Prefabs encapsulate all character behavior, making them easy to reuse or expand.

---

### 2. Fully Autonomous AI Flow

Characters act independently:
- Acquire random enemy targets
- Navigate toward them
- Shoot if in range and visible
- Track kills and update stats

---

### 3. Event-Based Lifecycle & Game State

- Characters detect death via `PlayerHealthBar`.
- On death:
  - They deactivate themselves
  - Notify `PlayerSpawner`
  - The attacker receives a kill increment
- `PlayerSpawner` checks remaining players and shows the final survivor using `UIManager`.

---

### 4. Real-Time Visual Feedback

- Health bars are UI sliders that:
  - Show real-time damage
  - Rotate to face the camera
  - Colorize based on health %
- Debug visuals (like raycast lines) help test visibility logic.
- Characters are visually identifiable using naming and spawn locations.

---

### 5. Editor-Friendly Setup

- Spawn points are manually placed and tagged in the scene.
- Configurable fields (attack speed, health, damage) are exposed in the Unity Inspector.
- Scripts like `PlayerSpawner` and `CameraUIButton` support quick iteration and camera control during testing.

---

###  Benefits of This Design

- **Decoupled and Clean** — each script does one job well.
- **Scalable** — supports any number of characters.
- **Extensible** — new logic can be added easily.
- **Debuggable** — logs, raycasts, and health bars offer complete runtime transparency.

---

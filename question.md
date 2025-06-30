# Que. 1- How would your code change if weapons had special effects, like the ability to make targets catch fire?

## Overview
If weapons had special effects, such as the ability to make targets catch fire on hit (e.g., BurnEffect), each weapon could maintain a list of possible effects it can apply.
On firing and successfully hitting a target, the weapon would trigger and apply one or more effects to the hit character.
A dedicated script, e.g., `EffectController.cs`, would be attached to each character to handle active effects like burning.

## Weapon Script Update

- Add a list of possible effects (`List<PossibleEffect> effects`) to each weapon.
- On hit, pass those effects to the target character's `EffectController`.

## EffectController.cs (new script for characters)

- Maintain a list of active effects (`List<ActiveEffect>`).
- Support starting, stacking, and removing effects based on timers or conditions.

### Example: Burn Effect

- Start a coroutine or timer.
- Periodically reduce health (e.g., every second).
- Trigger visual fire particle effects or burning animation.

## Integration with PlayerHealthBar

- `PlayerHealthBar.cs` should support taking damage from possible effects.
- Health reduction from effects should be distinguishable from regular damage (optional, for analytics or UI purposes).

---

# Que. 2 - How would you implement a dynamic alliance system in which combatants can form and disband teams during the battle?
## Overview
To implement a dynamic alliance system where combatants can form and disband teams during battle:
- Each character is assigned a Team ID (e.g., integer or string label), and characters will:
  - Not attack members of the same team
  - Re-evaluate alliances during gameplay
  - Be able to switch teams dynamically based on rules (e.g., proximity)

## Team Identity
Each character (`PlayerSetup`) will hold a field like:
```csharp
public int teamID;
```
This ID determines current alliance. Color-code team members using materials or indicators.

## Alliance Manager / Team Manager
A central system that tracks which players are in which teams.

### Responsibilities:
- Maintain a `Dictionary<int, List<PlayerSetup>>` mapping teamID to players
- Expose methods:
  ```csharp
  void FormTeam(int teamA, int teamB);
  void ChangeTeam(PlayerSetup player, int newTeamID);
  List<PlayerSetup> GetEnemies(PlayerSetup requester);
  ```

## Dynamic Targeting Logic
When a character finds a target, it should:
- Query `AllianceManager.GetEnemies(thisPlayer)` to filter only enemy players.
- This list updates in real-time if alliances change.

## Triggers for Team Changes
Characters can form or leave alliances based on in-game conditions.
*Examples:*
- **Merge Teams**: If two weak players are nearby and low on health, they may join the same team.
- **Proximity-based**: Players close together may form temporary alliances.

This logic can be implemented in `PlayerSetup.Update()`.

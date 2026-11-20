# OPAW Five-Card Battlefield Unit Roster

This package adds a five-card roster pager to the existing SCN03 battlefield MiniMap UI.

## Install
Copy the included `Assets` folder into the project root, preserving folder structure.

## Build
1. Open `SCN03_BATTLEFIELD_AAA`.
2. In Unity, run **Obsidian Protocol > Battlefield > Build Five-Card Unit Roster**.
3. Review the five cards and paging controls in the MiniMap hierarchy.
4. Save the scene only after reviewing the result.

The builder validates all five existing numbered card Images before making changes and uses Unity Undo.
The wide `Selected_Units` backing image and the `Units` label are left untouched.

## Runtime behavior
- Lists active scene objects carrying a component whose type name is `SelectableUnit`.
- Shows five deployed units per page, sorted by GameObject name.
- Clicking a card calls the existing UnitSelectionManager single-selection API by reflection.
- Paging changes only the roster view; it does not clear the manager's selection list.
- Refreshes the deployed roster on enable and when moving forward a page.

## Compatibility note
The runtime pager deliberately resolves `SelectableUnit` and `UnitSelectionManager` by type name to avoid assuming their namespaces. It tries the existing public/private `TrySingleSelect`, `SelectUnit`, or `Select` method with either a `SelectableUnit` component or its GameObject. If the project uses a different selection method signature, the console will report that it could not find a compatible method; no fallback mutation of the selection list is performed.

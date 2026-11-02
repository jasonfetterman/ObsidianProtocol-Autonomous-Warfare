OBSIDIAN PROTOCOL â€” MAIN MENU SAFE PATCH

IMPORTANT:
Do NOT use the old "Build Main Menu" builder. It creates a second scene.

1. Run this PowerShell script from the ROOT of your Unity project.
2. Return to Unity and let it compile.
3. Open your EXISTING SCN-01_MainMenu scene.
4. Use:
   Tools > Obsidian Protocol > Main Menu > SAFE PATCH EXISTING MENU

This patch:
- backs up the scene first
- does not delete GameObjects
- does not move GameObjects
- does not rename GameObjects
- does not create a second Main Menu scene
- reuses the existing Canvas/Main Menu
- reuses an existing EventSystem
- wires buttons that already exist
- does not guess destination scene names

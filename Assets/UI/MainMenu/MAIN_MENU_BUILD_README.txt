OBSIDIAN PROTOCOL - MAIN MENU

DO NOT manually rebuild the Main Menu hierarchy.

Unity Editor menu:

Tools
    >
Obsidian Protocol
    >
Build Main Menu

The builder creates:

SCN-01_MainMenu
|
+-- EventSystem
|
+-- Environment
|
+-- Canvas_MainMenuHUD
|   |
|   +-- PANEL_CenterMenu
|   |   +-- BUTTON_Continue
|   |   +-- BUTTON_Campaign
|   |   +-- BUTTON_Multiplayer
|   |   +-- BUTTON_Garage
|   |   +-- BUTTON_Store
|   |   +-- BUTTON_VROperator
|   |   +-- BUTTON_Settings
|   |   +-- BUTTON_Credits
|   |   +-- BUTTON_Exit
|   |
|   +-- PANEL_RightSystem
|   |
|   +-- PANEL_BottomBar
|   |
|   +-- POPUP_ExitConfirmation
|   |
|   +-- POPUP_PatchNotes
|   |
|   +-- POPUP_NetworkError
|   |
|   +-- POPUP_ProfileLogin
|
+-- EXTRA

IMPORTANT:

The Main Menu is the ENTRY POINT.

It does not contain:
- Garage internals
- Fleet internals
- Battlefield HUD
- Deployment internals
- Research internals

Those are separate systems.

The Main Menu only routes the player to those systems.

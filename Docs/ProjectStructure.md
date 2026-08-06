# ⚫ Project Structure — Obsidian Protocol

This document outlines the recommended folder structure for Obsidian Protocol: Autonomous Warfare.  
It ensures consistency, clarity, and scalability as the project grows.

---

## 📁 Root Structure

/ObsidianProtocol  
    /Docs  
    /Assets  
    /ProjectSettings  
    /Packages  

---

## 📚 Docs (Documentation)
Contains all design, planning, and reference documents.

- README.md  
- Units.md  
- Faction_WardenProgram.md  
- Roadmap.md  
- Lore.md  
- ProjectStructure.md  
- (Future) TechTrees.md  
- (Future) AI_Behavior.md  
- (Future) ModdingGuide.md  

---

## 🎮 Assets (Unity Project)

### Scripts
/Assets/Scripts
/RTS
/AI
/Units
/Buildings
/Economy
/MapSystem
/UI
/Abilities
/VR

### Models
/Assets/Models
/Units
/Air
/Ground
/Sea
/Command
/Experimental

### Other Asset Categories
/Assets/Textures
/Assets/Materials
/Assets/Audio
/Assets/Prefabs
/Assets/Scenes

---

## 🧠 Notes

- **Units** are organized by division (Air, Ground, Sea, Command, Experimental).  
- **Scripts** follow modular architecture — no monolithic systems.  
- **Docs** mirror the game’s conceptual structure for clarity.  
- **VR** is optional and isolated from core RTS logic.  
- **Future tools** (map editor, modding tools) will get their own folders.

---

## 🎯 Purpose

This structure ensures:

- Clean separation of systems  
- Easy onboarding for contributors  
- Scalable growth for future factions  
- Clear documentation for every part of the game  
- Professional presentation for GitHub and Steam  

Obsidian Protocol is built to grow — this structure keeps it stable as it expands.

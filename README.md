# 🌲 SlenderUma - Survival Horror (Slenderman Style) 🇺🇸 

A first-person psychological horror game focused on atmosphere, exploration, and survival, developed in **Unity 2022.3 LTS**. The main objective is the collection of objects (notes) in a vast and dense nighttime forest, all while the player is stalked by an entity.

## ⚙️ Technologies Used

* **Engine:** Unity 2022.3.62f3 (LTS)
* **Language:** C#/C++/OpenGL
* **Environment:** Unity Terrain System

## 🎮 Current Mechanics

* **First-Person Controller:** Fluid movement based on `CharacterController` with gravity physics.
* **Stamina and Flashlight System:** Speed management (walking/running) and an interactive system to turn the flashlight (Spotlight) on/off.
* **Immersive Atmosphere:**
* 500x500 terrain system with uneven elevation (valleys and hills).
* Optimized dense forest generation.
* Nighttime global illumination (Ambient Color set to black) and dim moonlight.
* Use of **Exponential Fog** to limit the player's vision and build psychological tension.



<img width="1009" height="538" alt="593606992-87ee949b-5718-40fd-b212-0ebdb49a9eba" src="https://github.com/user-attachments/assets/e4534c08-1aa1-4de4-b1c4-ed277497fd11" />



## 🕹️ Controls

| Action | Key |
| --- | --- |
| **Move** | `W` `A` `S` `D` |
| **Look around** | `Mouse` |
| **Sprint** | `Left Shift` (Hold) |
| **Flashlight (On/Off)** | `F` |
| **Interact** | `E` |

## 👥 Development Team / Credits

* **Delgadillo Jarquín Jurguen Adriel** - Lead development, C#, C++/OpenGL programming, and environment design
* **Miranda Ruiz Rodrigo Marcelo** - 3D model sourcing
* **Herrera Vásquez Carlos Eduardo** - Ambient sounds and sound effects
* **Mendez Alfaro Carlos David** - Texturing
* 

# Graphics Programming Project 2026 - SlenderUma

This repository contains the official C++ Launcher and the playable runtime build for **SlenderUma**, developed for the Computer Engineering Graphics Programming course.

## 🚀 Architecture Overview
- **Launcher (`Launcher.exe`):** A custom application written in **C++** utilizing **OpenGL** (via **GLFW** and **GLEW** libraries) to handle window contexts and execute the core game.
- **Game Build (`Juego/`):** The fully compiled interactive survival environment built in **Unity**.

---

## ⚠️ Critical Setup Instructions (Git LFS Required)
This project uses **Git LFS (Large File Storage)** to manage compressed game assets and large binary configurations. 

**DO NOT download this repository as a standard ZIP file** via the GitHub web interface. If you use "Download ZIP", GitHub will only download lightweight text metadata pointers, and the executable will fail to launch due to missing data.

### Correct Installation Steps:
1. Make sure you have **Git** and **Git LFS** installed on your system.
2. Open your terminal or Git Bash and clone the repository using the following command:
   ```bash
   git clone [https://github.com/Jurguen101/Proyecto-Programaci-n-Gr-fica-2026.git](https://github.com/Jurguen101/Proyecto-Programaci-n-Gr-fica-2026.git)
Wait for the download to finish completely. Git LFS will automatically pull the heavy asset files during the cloning process.

## 🎮 How to Run
Once the repository is successfully cloned to your local machine:

Open the project root folder (Proyecto-Programaci-n-Gr-fica-2026).

Run Launcher.exe (Double-click).

The custom C++ OpenGL application will launch and safely trigger the main game environment.

## 🛠️ Requirements & Dependencies

OS: Windows 10 / 11.

Graphics API: Dedicated or integrated GPU supporting stable OpenGL contexts.

Core DLLs: glew32.dll and glfw3.dll must remain in the same root directory as Launcher.exe to avoid runtime errors.

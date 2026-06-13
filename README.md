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

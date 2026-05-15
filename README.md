# 🌲 SlenderUma - Survival Horror (Slenderman Style)

Un juego de terror psicológico en primera persona enfocado en la atmósfera, la exploración y la supervivencia, desarrollado en **Unity 2022.3 LTS**. El objetivo principal es la recolección de objetos (notas) en un extenso y denso bosque nocturno, mientras el jugador es acechado por una entidad.

## ⚙️ Tecnologías Utilizadas
* **Motor:** Unity 2022.3.62f3 (LTS)
* **Lenguaje:** C#
* **Entorno:** Terrain System de Unity

## 🎮 Mecánicas Actuales
* **Controlador en Primera Persona:** Movimiento fluido basado en `CharacterController` con físicas de gravedad.
* **Sistema de Estamina y Linterna:** Gestión de velocidad (caminar/correr) y un sistema interactivo para encender/apagar la linterna (Spotlight).
* **Atmósfera Inmersiva:** 
  * Sistema de terreno de 500x500 con relieves irregulares (valles y colinas).
  * Generación de bosque denso optimizado.
  * Iluminación global nocturna (Ambient Color ajustado a negro) y luz lunar tenue.
  * Uso de **Niebla Exponencial (Exponential Fog)** para limitar la visión del jugador y generar tensión psicológica.

## 🕹️ Controles
| Acción | Tecla |
| :--- | :--- |
| **Moverse** | `W` `A` `S` `D` |
| **Mirar alrededor** | `Mouse` |
| **Correr** | `Left Shift` (Mantener) |
| **Linterna (On/Off)** | `F` |
| **Interactuar (Próximamente)** | `E` o `Click Izquierdo` |


Equipo de Desarrollo / Créditos
Delgadillo Jarquín Jurguen Adriel - Desarrollo principal, programación C# y diseño de entorno.

Miranda Ruiz Rodrigo Marcelo - Busqueda de Modelos 3D

Herrera Vásquez Carlos Eduardo - Sonidos ambiente y efectos de sonido

Mendez Alfaro Carlos David - Texturizado

#include <iostream>
#include <windows.h>
#include <mmsystem.h> // Librería para el audio nativo de Windows

// Le decimos al enlazador de Visual Studio que incluya la librería de audio
#pragma comment(lib, "winmm.lib")

// Ocultar la consola CMD de Windows en la versión final (Comenta esta línea si necesitas ver los std::cout para depurar)
#pragma comment(linker, "/SUBSYSTEM:windows /ENTRY:mainCRTStartup")

// GLEW siempre debe ir ANTES que GLFW
#include <GL/glew.h>
#include <GLFW/glfw3.h>

// Nuestras clases modulares
#include "Shader.h"
#include "Texture.h"

// Dimensiones de la ventana del Launcher (Ajustado para pantallas pequeñas)
//const GLint WIDTH = 600, HEIGHT = 600;

// ==========================================
// ESTRUCTURAS DE DATOS
// ==========================================
// Estructura para gestionar el estado del Launcher y compartirlo con el ratón
struct AppData {
    bool enMenuPrincipal;
    Texture* texturaActual;
    Texture* menuPtr;
    Texture* creditosPtr;
};

// ==========================================
// FUNCIONES SECUNDARIAS
// ==========================================

// Función nativa de Windows para abrir el ejecutable de Unity
void IniciarJuego(const char* rutaExe)
{
    STARTUPINFOA si;
    PROCESS_INFORMATION pi;

    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);
    ZeroMemory(&pi, sizeof(pi));

    if (CreateProcessA(rutaExe, NULL, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi))
    {
        CloseHandle(pi.hProcess);
        CloseHandle(pi.hThread);
    }
    else
    {
        std::cout << "[ERROR] No se pudo abrir: " << rutaExe << std::endl;
    }
}

void mouse_button_callback(GLFWwindow* window, int button, int action, int mods)
{
    if (button == GLFW_MOUSE_BUTTON_LEFT && action == GLFW_PRESS)
    {
        AppData* datos = (AppData*)glfwGetWindowUserPointer(window);
        if (datos == nullptr) return;

        double xpos, ypos;
        glfwGetCursorPos(window, &xpos, &ypos);

        // ==========================================
        // MAGIA DE RESOLUCIÓN VIRTUAL
        // ==========================================
        int winWidth, winHeight;
        glfwGetWindowSize(window, &winWidth, &winHeight); // Leemos el tamaño real

        // Traducimos el clic de la pantalla a nuestra escala de diseño (800x800)
        double xVirtual = (xpos / winWidth) * 800.0;
        double yVirtual = (ypos / winHeight) * 800.0;

        // ================================================================
        // LÓGICA MIENTRAS ESTAMOS EN EL MENÚ PRINCIPAL 
        // ================================================================
        if (datos->enMenuPrincipal)
        {
            // BOTÓN JUGAR (Usamos xVirtual y yVirtual en lugar de xpos y ypos)
            if (xVirtual >= 93 && xVirtual <= 364 && yVirtual >= 347 && yVirtual <= 427)
            {
                mciSendStringA("stop bgm", NULL, 0, NULL);
                mciSendStringA("close bgm", NULL, 0, NULL);
                glfwHideWindow(window);
                IniciarJuego("Juego/SlenderUma.exe");
                glfwSetWindowShouldClose(window, true);
            }

            // BOTÓN CRÉDITOS 
            else if (xVirtual >= 91 && xVirtual <= 363 && yVirtual >= 502 && yVirtual <= 586)
            {
                datos->enMenuPrincipal = false;
                datos->texturaActual = datos->creditosPtr;
                mciSendStringA("stop bgm", NULL, 0, NULL);
                mciSendStringA("open \"assets/Game Boy Horror.mp3\" type mpegvideo alias bgm_creditos", NULL, 0, NULL);
                mciSendStringA("play bgm_creditos repeat", NULL, 0, NULL);
            }

            // BOTÓN SALIR 
            else if (xVirtual >= 90 && xVirtual <= 362 && yVirtual >= 672 && yVirtual <= 754)
            {
                mciSendStringA("stop bgm", NULL, 0, NULL);
                mciSendStringA("close bgm", NULL, 0, NULL);
                glfwSetWindowShouldClose(window, true);
            }
        }

        // ================================================================
        // LÓGICA MIENTRAS ESTAMOS EN PANTALLA DE CRÉDITOS
        // ================================================================
        else
        {
            // BOTÓN "ATRAS" 
            if (xVirtual >= 90 && xVirtual <= 362 && yVirtual >= 672 && yVirtual <= 754)
            {
                datos->enMenuPrincipal = true;
                datos->texturaActual = datos->menuPtr;
                mciSendStringA("stop bgm_creditos", NULL, 0, NULL);
                mciSendStringA("close bgm_creditos", NULL, 0, NULL);
                mciSendStringA("play bgm from 0", NULL, 0, NULL);
            }
        }
    }
}

// ==========================================
// FUNCIÓN PRINCIPAL
// ==========================================
int main()
{
    // 0. DESACTIVAR LA "LUPA" DE WINDOWS (DPI AWARE)
    // Esto asegura que la ventana mida exactamente 800x800 píxeles reales en cualquier PC
    SetProcessDPIAware();

    // ==========================================
        // 1. INICIALIZAR GLFW Y VENTANA INTELIGENTE
        // ==========================================
        // Desactivamos la lupa de Windows
    SetProcessDPIAware();

    // Iniciamos la librería de ventanas (Si esto falta, nada funciona)
    if (!glfwInit())
    {
        MessageBoxA(NULL, "Error 1: No se pudo inicializar GLFW.", "ERROR FATAL", MB_OK | MB_ICONERROR);
        return -1;
    }

    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    glfwWindowHint(GLFW_RESIZABLE, GL_FALSE);

    // --- MAGIA RESPONSIVA BLINDADA ---
    int tamanoVentana = 600;
    int posX = 100;
    int posY = 100;

    GLFWmonitor* monitorPrincipal = glfwGetPrimaryMonitor();
    if (monitorPrincipal != nullptr)
    {
        const GLFWvidmode* modoPantalla = glfwGetVideoMode(monitorPrincipal);
        if (modoPantalla != nullptr)
        {
            tamanoVentana = (int)(modoPantalla->height * 0.85);
            posX = (modoPantalla->width - tamanoVentana) / 2;
            posY = (modoPantalla->height - tamanoVentana) / 2;
        }
    }

    // CREAR LA VENTANA
    GLFWwindow* window = glfwCreateWindow(tamanoVentana, tamanoVentana, "SlenderUma - Launcher", nullptr, nullptr);
    if (!window)
    {
        MessageBoxA(NULL, "Error 2: No se pudo crear la ventana de OpenGL.", "ERROR FATAL", MB_OK | MB_ICONERROR);
        glfwTerminate();
        return -1;
    }

    glfwSetWindowPos(window, posX, posY);
    glfwMakeContextCurrent(window);

    // ==========================================
    // 2. INICIALIZAR GLEW (Tarjeta Gráfica)
    // ==========================================
    glewExperimental = GL_TRUE;
    if (glewInit() != GLEW_OK)
    {
        MessageBoxA(NULL, "Error 3: No se pudo inicializar GLEW.", "ERROR FATAL", MB_OK | MB_ICONERROR);
        glfwDestroyWindow(window);
        glfwTerminate();
        return -1;
    }

    // Posicionar la ventana
    glfwSetWindowPos(window, posX, posY);

    // Activamos el contexto de OpenGL
    glfwMakeContextCurrent(window);

    // 2. INICIALIZAR GLEW
    glewExperimental = GL_TRUE;
    if (glewInit() != GLEW_OK)
    {
        glfwDestroyWindow(window);
        glfwTerminate();
        return -1;
    }

    int screenWidth, screenHeight;
    glfwGetFramebufferSize(window, &screenWidth, &screenHeight);
    glViewport(0, 0, screenWidth, screenHeight);

    // Transparencias para canales Alpha
    glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

    // 3. CARGAR RECURSOS Y GESTIONAR ESTADOS
    Shader launcherShader("assets/shaders/vertex.glsl", "assets/shaders/fragment.glsl");

    Texture fondoMenu("assets/textures/launcher_bg.png");
    Texture fondoCreditos("assets/textures/Credits_Bg.png");

    // Inicializamos la estructura de datos
    AppData datos;
    datos.enMenuPrincipal = true;
    datos.menuPtr = &fondoMenu;
    datos.creditosPtr = &fondoCreditos;
    datos.texturaActual = &fondoMenu;

    // Anclamos los datos a la ventana y activamos el ratón
    glfwSetWindowUserPointer(window, &datos);
    glfwSetMouseButtonCallback(window, mouse_button_callback);

    // 4. CONFIGURAR LA GEOMETRÍA (Vértices invertidos en V para corregir el giro)
    GLfloat vertices[] = {
        // Posición (X, Y)   // Coordenadas UV (U, V)
         1.0f,  1.0f,        1.0f, 0.0f, // Arriba Derecha
         1.0f, -1.0f,        1.0f, 1.0f, // Abajo Derecha
        -1.0f, -1.0f,        0.0f, 1.0f, // Abajo Izquierda
        -1.0f,  1.0f,        0.0f, 0.0f  // Arriba Izquierda
    };

    GLuint indices[] = {
        0, 1, 3,
        1, 2, 3
    };

    GLuint VAO, VBO, EBO;
    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO);
    glGenBuffers(1, &EBO);

    glBindVertexArray(VAO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(GLfloat), (GLvoid*)0);
    glEnableVertexAttribArray(0);
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 4 * sizeof(GLfloat), (GLvoid*)(2 * sizeof(GLfloat)));
    glEnableVertexAttribArray(1);

    glBindVertexArray(0);

    // 5. INICIAR MÚSICA DEL MENÚ PRINCIPAL
    mciSendStringA("open \"assets/Cancion del menu.mp3\" type mpegvideo alias bgm", NULL, 0, NULL);
    mciSendStringA("play bgm repeat", NULL, 0, NULL);

    // 6. BUCLE PRINCIPAL DE RENDERIZADO
    while (!glfwWindowShouldClose(window))
    {
        glfwPollEvents();

        glClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);

        launcherShader.Use();

        glActiveTexture(GL_TEXTURE0);

        // Dibujamos la textura dictada por el estado de la aplicación
        datos.texturaActual->Bind();

        glUniform1i(glGetUniformLocation(launcherShader.ID, "texture1"), 0);

        glBindVertexArray(VAO);
        glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
        glBindVertexArray(0);

        glfwSwapBuffers(window);
    }

    // 7. LIMPIEZA FINAL
    glDeleteVertexArrays(1, &VAO);
    glDeleteBuffers(1, &VBO);
    glDeleteBuffers(1, &EBO);

    glfwDestroyWindow(window);
    glfwTerminate();

    return 0;
}
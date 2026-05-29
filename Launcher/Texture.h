#pragma once
#include <GL/glew.h>
#include <SOIL2/SOIL2.h>
#include <iostream>

class Texture
{
public:
    GLuint ID;

    // Constructor que carga la imagen y genera la textura en OpenGL
    Texture(const char* imagePath)
    {
        glGenTextures(1, &ID);
        glBindTexture(GL_TEXTURE_2D, ID);

        // Configurar los parámetros de envoltura y filtrado
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
        // Filtrado lineal para mantener la calidad visual de la interfaz
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

        // Cargar la imagen con SOIL2 (forzando formato RGBA para soportar transparencias)
        int width, height, channels;
        unsigned char* image = SOIL_load_image(imagePath, &width, &height, &channels, SOIL_LOAD_RGBA);

        if (image)
        {
            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, image);
            glGenerateMipmap(GL_TEXTURE_2D);
        }
        else
        {
            std::cout << "ERROR::TEXTURE::Fallo al cargar la textura en la ruta: " << imagePath << std::endl;
        }

        // Liberar la memoria RAM (la imagen ya vive en la VRAM)
        SOIL_free_image_data(image);

        // Desvincular por seguridad
        glBindTexture(GL_TEXTURE_2D, 0);
    }

    // Activar la textura antes de dibujar
    void Bind()
    {
        glBindTexture(GL_TEXTURE_2D, ID);
    }
};
using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Models;
using NeuroSimulation.Utilities;

namespace NeuroSimulation;

class Program 
{
    static void Main() 
    {
        const int screenWidth = 1600;
        const int screenHeight = 900;
        
        Raylib.InitWindow(screenWidth, screenHeight, "Neuro Sim - Interactive Camera & Neuron");
        Raylib.SetTargetFPS(60);

        Vector2 somaCenter = new Vector2(400, 300);

        var soma = new NeuronSoma(somaCenter);
        var membrane = new Membrane(somaCenter);

        List<DendriteTree> dendritesList = new List<DendriteTree>();
        int dendriteCount = 4;
        float angleStep = MathF.PI / (dendriteCount + 1);        
        for (int i = 0; i < dendriteCount; i++) {
            float angle = MathF.PI * 0.7f + angleStep * (i + 1);
            dendritesList.Add(NeuronUtils.GenerateRandomDendrite(angle));
        }

        var axon = NeuronUtils.GenerateRandomAxon(0.0f);

        Camera2D camera = new Camera2D();
        camera.Target = somaCenter;
        camera.Offset = new Vector2(screenWidth / 2f, screenHeight / 2f);
        camera.Rotation = 0.0f;
        camera.Zoom = 1.0f;

        Color darkPurpleBg = new Color(26, 15, 35, 255);

        float simTime = 0.0f;
        while (!Raylib.WindowShouldClose()) 
        {
            float dt = Raylib.GetFrameTime();
            simTime += dt;
            soma.Update(simTime);

            float wheel = Raylib.GetMouseWheelMove();
            if (wheel != 0.0f) 
            {
                camera.Zoom += wheel * 0.1f;
                if (camera.Zoom < 0.2f) camera.Zoom = 0.2f;
                if (camera.Zoom > 5.0f) camera.Zoom = 5.0f;
            }

            if (Raylib.IsMouseButtonDown(MouseButton.Left) || Raylib.IsMouseButtonDown(MouseButton.Middle)) 
            {
                Vector2 delta = Raylib.GetMouseDelta();
                camera.Target -= delta / camera.Zoom;
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(darkPurpleBg);
            Raylib.BeginMode2D(camera);

            foreach (var d in dendritesList) 
            {
                d.Draw(soma.Center, soma.RadiusX, soma.RadiusY);
            }
            axon.Draw(soma.Center, soma.RadiusX, soma.RadiusY);
            soma.Draw(simTime);
            membrane.Draw(soma.RadiusX, soma.RadiusY);

            Raylib.EndMode2D();

            Raylib.DrawText("Controls: Drag LKM/Middle Mouse to Move | Wheel to Zoom", 20, 20, 16, new Color(200, 170, 220, 255));

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
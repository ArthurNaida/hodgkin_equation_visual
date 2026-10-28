using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Models;
using NeuroSimulation.Utilities;
using NeuroSimulation.Behaviors;

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
        var viewController = new InteractiveViewController();

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

            viewController.ZoomCanvas(ref camera);
            viewController.MoveCanvas(ref camera);

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

            Vector2 axonTipPos = somaCenter + new Vector2(300, 0); 

            viewController.ShowWorldLabels(somaCenter, axonTipPos, dendritesList.Count);

            Raylib.EndMode2D();

            viewController.ShowControlsHint();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
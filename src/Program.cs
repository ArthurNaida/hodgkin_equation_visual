using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Models;
using NeuroSimulation.Utilities;
using NeuroSimulation.Behaviors;
using NeuroSimulation.UI;

namespace NeuroSimulation;

class Program 
{
    static void Main(string[] args) 
    {
        const int screenWidth = 1600;
        const int screenHeight = 900;
        
        Raylib.InitWindow(screenWidth, screenHeight, "Neuro Sim - Hodgkin-Huxley Design System HUD");
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
        var simulationLogger = new SimulationLogger();
        
        var physicsSettingsHud = new PhysicsSettingsHud();

        var p = new HodgkinHuxleyParameters();
        var neuron = new Neuron(-65.0, 0.05, 0.6, 0.32);

        float visualTime = 0.0f;

        double simTime = 0.0;

        const float logInterval = 0.5f;
        float logTimer = 0.0f;

        Camera2D camera = new Camera2D();
        camera.Target = somaCenter;
        camera.Offset = new Vector2(screenWidth / 2f, screenHeight / 2f);
        camera.Rotation = 0.0f;
        camera.Zoom = 1.0f;

        Color darkPurpleBg = new Color(26, 15, 35, 255);

        simulationLogger.AddLog(simTime, neuron.V);

        while (!Raylib.WindowShouldClose()) 
        {
            float frameDt = Raylib.GetFrameTime();

            visualTime += frameDt;
            soma.Update(visualTime);

            double currentExternalCurrent = 20.0;
            double currentDt = 0.01; // Шаг солвера из HUD (от 0.01 до 0.10)
            bool exportClicked = false;

            physicsSettingsHud.Draw(screenWidth, ref p, ref currentExternalCurrent, ref currentDt, out exportClicked);

            if (exportClicked || Raylib.IsKeyPressed(KeyboardKey.F5)) 
            {
                simulationLogger.ExportToFile("simulation_export.txt");
                Console.WriteLine("[Export] Logs successfully saved to simulation_export.txt");
            }

            logTimer += frameDt;
            if (logTimer >= logInterval) 
            {
                logTimer -= logInterval;
                simTime += currentDt;
                double iExt = (simTime >= 1.0 && simTime <= 3.0) ? currentExternalCurrent : 0.0;

                var nextNeuron = HodgkinHuxleyModel.StepRK4(neuron, p, iExt, currentDt);
                if (!double.IsNaN(nextNeuron.V) && !double.IsInfinity(nextNeuron.V)) 
                {
                    neuron = nextNeuron;
                }

                simulationLogger.AddLog(simTime, neuron.V);
            }

            viewController.ZoomCanvas(ref camera);
            viewController.MoveCanvas(ref camera);

            // --- DRAW PHASE ---
            Raylib.BeginDrawing();
            Raylib.ClearBackground(darkPurpleBg);
            
            Raylib.BeginMode2D(camera);

            foreach (var d in dendritesList) 
            {
                d.Draw(soma.Center, soma.RadiusX, soma.RadiusY);
            }
            axon.Draw(soma.Center, soma.RadiusX, soma.RadiusY);
            soma.Draw(visualTime);
            membrane.Draw(soma.RadiusX, soma.RadiusY);

            Vector2 axonTipPos = somaCenter + new Vector2(300, 0); 
            viewController.ShowWorldLabels(somaCenter, axonTipPos, dendritesList.Count);

            Raylib.EndMode2D();

            viewController.ShowControlsHint();
            LogUtils.DrawLogWindow(screenWidth, simulationLogger);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
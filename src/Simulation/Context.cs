using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Models;
using NeuroSimulation.Behaviors;
using NeuroSimulation.UI;
using NeuroSimulation.Utils;

namespace NeuroSimulation.Simulation;

public class Context
{
    public const int ScreenWidth = 1600;
    public const int ScreenHeight = 900;

    public Vector2 SomaCenter { get; } = new Vector2(400, 300);

    public NeuronSoma Soma { get; }
    public Membrane Membrane { get; }
    public List<DendriteTree> DendritesList { get; } = new();
    public Axon Axon { get; }
    public InteractiveViewController ViewController { get; } = new();
    public SimulationLogger SimulationLogger { get; } = new();
    public PhysicsSettingsHud PhysicsSettingsHud { get; } = new();
    
    public HodgkinHuxleyParameters Parameters = new();
    public Neuron Neuron { get; set; } = new(-65.0, 0.05, 0.6, 0.32);
    public Electrode Electrode { get; }

    public Camera2D Camera;
    public Color DarkPurpleBg { get; } = new(26, 15, 35, 255);

    public float VisualTime { get; set; } = 0.0f;
    public double SimTime { get; set; } = 0.0;
    
    public double ExternalCurrent = 20.0;
    public double CurrentDt = 0.01;
    public bool WasStimulating = false;

    public float LogTimer = 0.0f;
    public const float LogInterval = 0.5f;

    public Context()
    {
        Soma = new NeuronSoma(SomaCenter);
        Membrane = new Membrane(SomaCenter);
        Electrode = new Electrode(SomaCenter, Soma.RadiusX, Soma.RadiusY, appliedCurrent: 25.0f);

        int dendriteCount = 4;
        float angleStep = MathF.PI / (dendriteCount + 1);        
        for (int i = 0; i < dendriteCount; i++) {
            float angle = MathF.PI * 0.7f + angleStep * (i + 1);
            DendritesList.Add(NeuronUtils.GenerateRandomDendrite(angle));
        }

        Axon = NeuronUtils.GenerateRandomAxon(0.0f);

        Camera = new Camera2D
        {
            Target = SomaCenter,
            Offset = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f),
            Rotation = 0.0f,
            Zoom = 1.0f
        };

        SimulationLogger.AddLog(SimTime, Neuron.V);
    }
}
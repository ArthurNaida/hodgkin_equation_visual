using System;
using System.Numerics;
using NeuroSimulation.Behaviors;

namespace NeuroSimulation.Models;

public class NeuronSoma 
{
    public Vector2 Center;
    
    public float RadiusX { get; private set; } = 90f;
    public float RadiusY { get; private set; } = 60f;

    private const float BaseRadiusX = 90f;
    private const float BaseRadiusY = 60f;

    public NeuronSoma(Vector2 center) 
    {
        Center = center;
    }

    public void Update(float time, double voltage) 
    {
        var radii = SomaVisualRenderer.DynamicsUpdate(BaseRadiusX, BaseRadiusY, time, voltage);
        RadiusX = radii.radiusX;
        RadiusY = radii.radiusY;
    }

    public void Draw(float time) 
    {
        SomaVisualRenderer.DrawSomaLayers(Center, RadiusX, RadiusY);
        SomaVisualRenderer.DrawSomaBlobs(Center, RadiusX, RadiusY, time);
    }
}
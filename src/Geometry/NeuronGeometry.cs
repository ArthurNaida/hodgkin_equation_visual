using System;
using System.Numerics;

namespace NeuroSimulation.Geometry;

public static class NeuronGeometry 
{
    public static Vector2 GetEllipseBorderPoint(Vector2 center, float rx, float ry, float angle) 
    {
        return new Vector2(
            center.X + rx * MathF.Cos(angle),
            center.Y + ry * MathF.Sin(angle)
        );
    }
}
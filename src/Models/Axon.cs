using System;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Geometry;

namespace NeuroSimulation.Models;

public class Axon 
{
    public float Angle;       
    public float Curvature;   
    public float Length;      
    public Color ColorVal = new Color(255, 200, 100, 255);

    public Axon(float angle, float curvature, float length) 
    {
        Angle = angle;
        Curvature = curvature;
        Length = length;
    }

    public void Draw(Vector2 somaCenter, float rx, float ry) 
    {
        Vector2 startPos = NeuronGeometry.GetEllipseBorderPoint(somaCenter, rx, ry, Angle);

        float cosA = MathF.Cos(Angle);
        float sinA = MathF.Sin(Angle);
        Vector2 outward = new Vector2(cosA, sinA);
        Vector2 tangent = new Vector2(-sinA, cosA);

        Vector2 p1 = startPos + outward * (Length * 0.25f) + tangent * (30f * Curvature);
        Vector2 p2 = startPos + outward * (Length * 0.50f) - tangent * (35f * Curvature);
        Vector2 p3 = startPos + outward * (Length * 0.75f) + tangent * (20f * Curvature);
        Vector2 p4 = startPos + outward * Length;

        Raylib.DrawLineEx(startPos, p1, 5.5f, ColorVal);

        Vector2[] axonPoints = { startPos, p1, p2, p3, p4 };
        Raylib.DrawSplineCatmullRom(axonPoints, 5, 5.5f, ColorVal);
    }
}
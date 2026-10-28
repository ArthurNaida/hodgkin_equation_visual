using System;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Geometry;

namespace NeuroSimulation.Models;

public class DendriteTree 
{
    public float Angle; 
    public float Curvature; 
    public int SubCount; 
    public Color ColorVal = new Color(255, 140, 0, 255);

    public DendriteTree(float angle, float curvature, int subCount) 
    {
        Angle = angle;
        Curvature = curvature;
        SubCount = subCount;
    }

    public void Draw(Vector2 somaCenter, float rx, float ry) 
    {
        Vector2 startPos = NeuronGeometry.GetEllipseBorderPoint(somaCenter, rx, ry, Angle);

        float cosA = MathF.Cos(Angle);
        float sinA = MathF.Sin(Angle);
        Vector2 outward = new Vector2(cosA, sinA);
        Vector2 tangent = new Vector2(-sinA, cosA);

        float totalLength = 100f + SubCount * 25f;

        Vector2 node1 = startPos + outward * (totalLength * 0.35f) + tangent * (15f * Curvature);
        Vector2 node2 = startPos + outward * (totalLength * 0.7f) - tangent * (20f * Curvature);
        Vector2 node3 = startPos + outward * totalLength + tangent * (5f * Curvature);

        Raylib.DrawLineEx(startPos, node1, 4.0f, ColorVal);

        Vector2[] mainBranch = { startPos, node1, node2, node3 };
        Raylib.DrawSplineCatmullRom(mainBranch, 4, 4.0f, ColorVal);

        for (int i = 0; i < SubCount; i++) 
        {
            float t = (SubCount == 1) ? 0.5f : (float)i / (SubCount - 1);
            Vector2 anchorNode = Vector2.Lerp(node1, node2, t);

            float side = (i % 2 == 0) ? 1f : -1f;

            Vector2 subStart = anchorNode;
            Vector2 subP2 = subStart + outward * 25f + tangent * (20f * Curvature * side);
            Vector2 subP3 = subP2 + outward * 25f + tangent * (10f * Curvature * side);

            Vector2[] subBranch = { subStart, subStart, subP2, subP3 };
            Raylib.DrawSplineCatmullRom(subBranch, 4, 2.5f, ColorVal);
        }
    }
}
using System.Numerics;
using Raylib_cs;

namespace NeuroSimulation.Models;

public class Membrane 
{
    public Vector2 Center;
    public Color ColorVal = new Color(255, 220, 130, 255);

    public Membrane(Vector2 center) 
    {
        Center = center;
    }

    public void Draw(float radiusX, float radiusY) 
    {
        for (int i = 0; i < 4; i++) 
        {
            Raylib.DrawEllipseLines((int)Center.X, (int)Center.Y, radiusX + i, radiusY + i, ColorVal);
        }
    }
}
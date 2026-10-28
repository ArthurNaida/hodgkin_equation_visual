using System;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Behaviors;

namespace NeuroSimulation.Models;

public class NeuronSoma 
{
    public Vector2 Center;
    private readonly EllipsePulseDynamics dynamics;
    
    public float RadiusX => dynamics.RadiusX;
    public float RadiusY => dynamics.RadiusY;

    public NeuronSoma(Vector2 center) 
    {
        Center = center;
        dynamics = new EllipsePulseDynamics(90f, 60f);
    }

    public void Update(float time) 
    {
        dynamics.Update(time);
    }

    public void Draw(float time) 
    {
        int bgLayers = 40;
        for (int i = bgLayers; i >= 0; i--) 
        {
            float t = (float)i / bgLayers;
            float rx = RadiusX * t;
            float ry = RadiusY * t;

            float density = t * t * (3f - 2f * t);

            int r = (int)(140 + 20 * density);
            int g = (int)(60 + 10 * density);
            int b = (int)(10 + 5 * density);

            Raylib.DrawEllipse((int)Center.X, (int)Center.Y, rx, ry, new Color(r, g, b, 255));
        }

        
        int blobCount = 4;
        for (int i = 0; i < blobCount; i++) 
        {
            Vector2 blobCenter = dynamics.GetBlobCenter(Center, i, time);
            float lifeCycle = dynamics.GetBlobLifeCycle(time, i);
            
            float blobRx = RadiusX * 0.65f;
            float blobRy = RadiusY * 0.65f;

            int subLayers = 25;
            for (int j = subLayers; j >= 1; j--) 
            {
                float subT = (float)j / subLayers;                 
                float alphaProfile = MathF.Sin(subT * MathF.PI); 

                Color blobColor = new Color(
                    210, 
                    150, 
                    50, 
                    (int)(7 * lifeCycle * alphaProfile)
                );

                Raylib.DrawEllipse(
                    (int)blobCenter.X, 
                    (int)blobCenter.Y, 
                    blobRx * subT, 
                    blobRy * subT, 
                    blobColor
                );
            }
        }
    }
}
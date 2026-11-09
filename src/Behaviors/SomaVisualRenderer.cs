using System;
using System.Numerics;
using Raylib_cs;

namespace NeuroSimulation.Behaviors;

public static class SomaVisualRenderer 
{
    private static float smoothActivation = 0.0f;

    public static (float radiusX, float radiusY) DynamicsUpdate(float baseRadiusX, float baseRadiusY, float time, double voltage)
    {
        float targetActivation = 0.0f;
        if (voltage > -50.0)
        {
            targetActivation = (float)Math.Clamp((voltage - (-50.0)) / (40.0 - (-50.0)), 0.0, 1.0);
        }

        smoothActivation = ScalarLerp(smoothActivation, targetActivation, 0.12f);

        float pulseX = MathF.Sin(time * 1.5f) * 6f + MathF.Cos(time * 0.8f) * 3f;
        float pulseY = MathF.Cos(time * 1.3f) * 6f + MathF.Sin(time * 0.9f) * 3f;

        float spikeExpansion = smoothActivation * 18f; 

        float rx = baseRadiusX + pulseX + spikeExpansion;
        float ry = baseRadiusY + pulseY + spikeExpansion;

        return (rx, ry);
    }

    public static Vector2 GetDynamicsBlobCenter(Vector2 somaCenter, int index, float time, float radiusX, float radiusY)
    {
        float baseAngle = index * (MathF.PI * 0.5f);
        float localOffsetAngle = baseAngle + MathF.Sin(time * 0.3f + index) * 0.3f;
        float distanceX = radiusX * 0.45f;
        float distanceY = radiusY * 0.45f;

        return somaCenter + new Vector2(
            MathF.Cos(localOffsetAngle) * distanceX,
            MathF.Sin(localOffsetAngle) * distanceY
        );
    }

    public static float GetDynamicsBlobLifeCycle(float time, int index)
    {
        return 0.3f + 0.7f * MathF.Abs(MathF.Sin(time * 0.25f + index * 1.5f));
    }

    public static void DrawSomaLayers(Vector2 center, float radiusX, float radiusY) 
    {
        int bgLayers = 40;
        for (int i = bgLayers; i >= 0; i--) 
        {
            float t = (float)i / bgLayers;
            float rx = radiusX * t;
            float ry = radiusY * t;

            float density = t * t * (3f - 2f * t);

            float restR = 175 + 25 * density;
            float restG = 65 + 15 * density;
            float restB = 15 + 5 * density;

            float peakR = 20 + 10 * density;
            float peakG = 100 + 40 * density;
            float peakB = 245 + 10 * density;

            int r = (int)(restR * (1.0f - smoothActivation) + peakR * smoothActivation);
            int g = (int)(restG * (1.0f - smoothActivation) + peakG * smoothActivation);
            int b = (int)(restB * (1.0f - smoothActivation) + peakB * smoothActivation);

            Raylib.DrawEllipse((int)center.X, (int)center.Y, rx, ry, new Color(r, g, b, 255));
        }
    }

    public static void DrawSomaBlobs(Vector2 center, float radiusX, float radiusY, float time) 
    {
        int blobCount = 4;
        for (int i = 0; i < blobCount; i++) 
        {
            Vector2 blobCenter = GetDynamicsBlobCenter(center, i, time, radiusX, radiusY);
            float lifeCycle = GetDynamicsBlobLifeCycle(time, i);
            
            float blobRx = radiusX * 0.65f;
            float blobRy = radiusY * 0.65f;

            int subLayers = 25;
            for (int j = subLayers; j >= 1; j--) 
            {
                float subT = (float)j / subLayers;                 
                float alphaProfile = MathF.Sin(subT * MathF.PI); 

                int colR = (int)(210 * (1.0f - smoothActivation) + 255 * smoothActivation);
                int colG = (int)(130 * (1.0f - smoothActivation) + 235 * smoothActivation);
                int colB = (int)(30  * (1.0f - smoothActivation) + 80  * smoothActivation);

                Color blobColor = new Color(
                    colR, 
                    colG, 
                    colB, 
                    (int)((7 + 15 * smoothActivation) * lifeCycle * alphaProfile)
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

    private static float ScalarLerp(float start, float end, float amount) 
    {
        return start + (end - start) * amount;
    }
}
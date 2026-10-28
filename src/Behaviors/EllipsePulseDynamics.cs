using System;
using System.Numerics;

namespace NeuroSimulation.Behaviors;

public class EllipsePulseDynamics
{
    private readonly float baseRadiusX;
    private readonly float baseRadiusY;

    public float RadiusX { get; private set; }
    public float RadiusY { get; private set; }

    public EllipsePulseDynamics(float baseRadiusX, float baseRadiusY)
    {
        this.baseRadiusX = baseRadiusX;
        this.baseRadiusY = baseRadiusY;
        RadiusX = baseRadiusX;
        RadiusY = baseRadiusY;
    }

    public void Update(float time)
    {
        // Плавная органическая пульсация сомы
        RadiusX = baseRadiusX + MathF.Sin(time * 1.5f) * 6f + MathF.Cos(time * 0.8f) * 3f;
        RadiusY = baseRadiusY + MathF.Cos(time * 1.3f) * 6f + MathF.Sin(time * 0.9f) * 3f;
    }

    public Vector2 GetBlobCenter(Vector2 somaCenter, int index, float time)
    {
        float baseAngle = index * (MathF.PI * 0.5f);
        float localOffsetAngle = baseAngle + MathF.Sin(time * 0.3f + index) * 0.3f;
        float distanceX = RadiusX * 0.45f;
        float distanceY = RadiusY * 0.45f;

        return somaCenter + new Vector2(
            MathF.Cos(localOffsetAngle) * distanceX,
            MathF.Sin(localOffsetAngle) * distanceY
        );
    }

    public float GetBlobLifeCycle(float time, int index)
    {
        return 0.3f + 0.7f * MathF.Abs(MathF.Sin(time * 0.25f + index * 1.5f));
    }
}
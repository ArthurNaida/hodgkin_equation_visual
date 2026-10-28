using System;
using NeuroSimulation.Models;

namespace NeuroSimulation.Utilities;

public static class NeuronUtils 
{
    private static readonly Random Rand = new Random();

    public static DendriteTree GenerateRandomDendrite(float angle, float minCurvature = 0.8f, float maxCurvature = 1.6f) 
    {
        float curvature = minCurvature + (float)Rand.NextDouble() * (maxCurvature - minCurvature);
        int subCount = 2 + Rand.Next(3); 
        
        return new DendriteTree(angle, curvature, subCount);
    }

    public static Axon GenerateRandomAxon(float angle, float minLength = 200f, float maxLength = 320f) 
    {
        float curvature = 0.5f + (float)Rand.NextDouble() * 0.8f;
        float length = minLength + (float)Rand.NextDouble() * (maxLength - minLength);

        return new Axon(angle, curvature, length);
    }
}
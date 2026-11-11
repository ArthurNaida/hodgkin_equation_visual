using System;
using Raylib_cs;
using NeuroSimulation.Models;

namespace NeuroSimulation.Simulation;

public static class Visual
{
    public static void Update(Context ctx, float frameDt)
    {
        ctx.VisualTime += frameDt;
        ctx.Soma.Update(ctx.VisualTime, ctx.Neuron.V);

        bool exportClicked = false;
        ctx.PhysicsSettingsHud.Draw(Context.ScreenWidth, ref ctx.Parameters, ref ctx.ExternalCurrent, ref ctx.CurrentDt, out exportClicked);
        
        if (exportClicked || Raylib.IsKeyPressed(KeyboardKey.F5)) 
        {
            ctx.SimulationLogger.ExportToFile("simulation_export.txt");
            Console.WriteLine("[Export] Logs successfully saved to simulation_export.txt");
        }
        
        bool isStimulating = ctx.Electrode.Update(ctx.Soma.Center, ctx.Soma.RadiusX, ctx.Soma.RadiusY, ctx.Camera);
        double iExt = (isStimulating || ctx.SimTime < 3) ? ctx.ExternalCurrent : 0.0;

        ctx.LogTimer += frameDt;
        if (ctx.LogTimer >= Context.LogInterval) 
        {
            ctx.LogTimer -= Context.LogInterval;
            ctx.SimTime += ctx.CurrentDt;

            var nextNeuron = HodgkinHuxleyModel.StepRK4(ctx.Neuron, ctx.Parameters, iExt, ctx.CurrentDt);
            if (!double.IsNaN(nextNeuron.V) && !double.IsInfinity(nextNeuron.V)) 
            {
                ctx.Neuron = nextNeuron;
            }

            ctx.SimulationLogger.AddLog(ctx.SimTime, ctx.Neuron.V);

            if (isStimulating && !ctx.WasStimulating)
            {
                ctx.SimulationLogger.AddMessage($"[Stimulation] Electrode triggered at t = {ctx.SimTime:F2}s with current = {ctx.ExternalCurrent} uA");
            }
            ctx.WasStimulating = isStimulating;
        }

        ctx.ViewController.ZoomCanvas(ref ctx.Camera);
        ctx.ViewController.MoveCanvas(ref ctx.Camera);
    }
}
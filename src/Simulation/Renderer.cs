using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Utils;
using NeuroSimulation.UI;

namespace NeuroSimulation.Simulation;

public static class Renderer
{
    public static void Draw(Context ctx)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(ctx.DarkPurpleBg);
        
        Raylib.BeginMode2D(ctx.Camera);

        foreach (var d in ctx.DendritesList) 
        {
            d.Draw(ctx.Soma.Center, ctx.Soma.RadiusX, ctx.Soma.RadiusY);
        }
        ctx.Axon.Draw(ctx.Soma.Center, ctx.Soma.RadiusX, ctx.Soma.RadiusY);
        ctx.Soma.Draw(ctx.VisualTime);
        ctx.Membrane.Draw(ctx.Soma.RadiusX, ctx.Soma.RadiusY);
        ctx.Electrode.DrawWorld();

        Vector2 axonTipPos = ctx.SomaCenter + new Vector2(300, 0); 
        ctx.ViewController.ShowWorldLabels(ctx.SomaCenter, axonTipPos, ctx.DendritesList.Count);

        Raylib.EndMode2D();

        ctx.ViewController.ShowControlsHint();
        ctx.Electrode.DrawUI();
        LogUtils.DrawLogWindow(Context.ScreenWidth, ctx.SimulationLogger);
        OscilloscopeGraph.Draw(Context.ScreenHeight, ctx.SimulationLogger);

        Raylib.EndDrawing();
    }
}
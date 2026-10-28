using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Models;

namespace NeuroSimulation.Behaviors;

public class InteractiveViewController 
{
    private readonly Color textColor = new Color(200, 170, 220, 255);
    private readonly Color labelColor = new Color(255, 220, 150, 220);

    public void ZoomCanvas(ref Camera2D camera) 
    {
        float wheel = Raylib.GetMouseWheelMove();
        if (wheel != 0.0f) 
        {
            camera.Zoom += wheel * 0.1f;
            if (camera.Zoom < 0.2f) camera.Zoom = 0.2f;
            if (camera.Zoom > 5.0f) camera.Zoom = 5.0f;
        }
    }

    public void MoveCanvas(ref Camera2D camera) 
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left) || Raylib.IsMouseButtonDown(MouseButton.Middle)) 
        {
            Vector2 delta = Raylib.GetMouseDelta();
            camera.Target -= delta / camera.Zoom;
        }
    }

    public void ShowControlsHint() 
    {
        bool showHints = Raylib.IsKeyDown(KeyboardKey.LeftAlt) || Raylib.IsKeyDown(KeyboardKey.RightAlt);

        if (!showHints) 
        {
            Raylib.DrawText("Hold [Alt] for controls & labels", 20, 20, 16, textColor);
        }
        else 
        {
            Raylib.DrawText("Controls: Drag LKM/Middle Mouse to Move | Wheel to Zoom", 20, 20, 16, textColor);
        }
    }

    public void ShowWorldLabels(Vector2 somaCenter, Vector2 axonEndPos, int dendriteCount) 
    {
        bool showHints = Raylib.IsKeyDown(KeyboardKey.LeftAlt) || Raylib.IsKeyDown(KeyboardKey.RightAlt);
        if (!showHints) return;

        Raylib.DrawText("Soma", (int)somaCenter.X - 18, (int)somaCenter.Y - 10, 18, labelColor);

        Raylib.DrawText("Axon", (int)axonEndPos.X - 15, (int)axonEndPos.Y - 20, 18, labelColor);

        float angleStep = MathF.PI / (dendriteCount + 1);
        for (int i = 0; i < dendriteCount; i++) 
        {
            float angle = MathF.PI * 0.7f + angleStep * (i + 1);
            Vector2 tipPos = somaCenter + new Vector2(MathF.Cos(angle) * 150f, MathF.Sin(angle) * 150f);

            Raylib.DrawText($"Dendrite {i + 1}", (int)tipPos.X - 30, (int)tipPos.Y - 20, 16, labelColor);
        }
    }
}
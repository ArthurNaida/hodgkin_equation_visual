using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.Behaviors;

namespace NeuroSimulation.UI;

public static class OscilloscopeGraph 
{
    private const double MinVoltage = -90.0;
    private const double MaxVoltage = 50.0;
    private const double VoltageRange = MaxVoltage - MinVoltage;

    public static void Draw(int screenHeight, SimulationLogger logger) 
    {
        Draw(screenHeight, logger?.History);
    }

    public static void Draw(int screenHeight, IReadOnlyList<(double Time, double Voltage)>? points) 
    {
        const int width = 450;
        const int height = 300;
        const int x = 40;
        int y = screenHeight - height - 50;

        UIWindow.Draw(x, y, width, height, "Action Potential");

        const int paddingLeft = 52;
        const int paddingRight = 18;
        const int paddingTop = 44;
        const int paddingBottom = 28;

        float plotX = x + paddingLeft;
        float plotY = y + paddingTop;
        float plotWidth = width - paddingLeft - paddingRight;
        float plotHeight = height - paddingTop - paddingBottom;

        Raylib.DrawRectangle((int)plotX, (int)plotY, (int)plotWidth, (int)plotHeight, new Color(12, 10, 20, 240));
        Raylib.DrawRectangleLines((int)plotX, (int)plotY, (int)plotWidth, (int)plotHeight, new Color(70, 50, 95, 255));

        double[] referenceVoltages = { 50.0, 0.0, -65.0, -90.0 };
        Color gridColor = new Color(55, 40, 75, 180);
        Color restLineColor = new Color(85, 60, 115, 200);
        Color labelColor = new Color(190, 170, 220, 220);

        Raylib.DrawText("mV", (int)plotX - 28, (int)plotY - 14, 10, new Color(255, 220, 150, 220));

        foreach (double v in referenceVoltages) 
        {
            float normY = (float)((v - MinVoltage) / VoltageRange);
            float lineY = plotY + plotHeight * (1.0f - normY);

            Color lineCol = (v == -65.0) ? restLineColor : gridColor;
            Raylib.DrawLine((int)plotX, (int)lineY, (int)(plotX + plotWidth), (int)lineY, lineCol);

            string vLabel = v > 0 ? $"+{v:0}" : $"{v:0}";
            int textW = Raylib.MeasureText(vLabel, 10);
            Raylib.DrawText(vLabel, (int)(plotX - textW - 6), (int)(lineY - 5), 10, labelColor);
        }

        if (points == null || points.Count == 0) 
        {
            string waitingMsg = "Awaiting simulation data...";
            int msgW = Raylib.MeasureText(waitingMsg, 11);
            Raylib.DrawText(waitingMsg, (int)(plotX + (plotWidth - msgW) / 2), (int)(plotY + (plotHeight - 11) / 2), 11, new Color(130, 110, 160, 200));
            return;
        }

        double minTime = points[0].Time;
        double maxTime = points[^1].Time;
        double timeSpan = Math.Max(0.5, maxTime - minTime);

        string tStart = $"{minTime:F1}s";
        string tEnd = $"{maxTime:F1}s";
        Raylib.DrawText(tStart, (int)plotX, (int)(plotY + plotHeight + 6), 10, labelColor);
        int tEndW = Raylib.MeasureText(tEnd, 10);
        Raylib.DrawText(tEnd, (int)(plotX + plotWidth - tEndW), (int)(plotY + plotHeight + 6), 10, labelColor);

        string timeAxisLabel = "Time";
        int timeLabelW = Raylib.MeasureText(timeAxisLabel, 10);
        Raylib.DrawText(timeAxisLabel, (int)(plotX + (plotWidth - timeLabelW) / 2), (int)(plotY + plotHeight + 6), 10, new Color(140, 120, 170, 200));

        double currentVoltage = points[^1].Voltage;
        Color neonCyan = new Color(0, 245, 230, 255);
        string vHeader = $"{currentVoltage:+0.0;-0.0;0.0} mV";
        int vHeaderW = Raylib.MeasureText(vHeader, 12);
        Raylib.DrawText(vHeader, (int)(x + width - vHeaderW - 15), (int)(y + 12), 12, neonCyan);

        Vector2[] screenPoints = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++) 
        {
            var (t, v) = points[i];
            float normX = (float)((t - minTime) / timeSpan);
            float normY = (float)((v - MinVoltage) / VoltageRange);
            normY = Math.Clamp(normY, 0.0f, 1.0f);

            float px = plotX + normX * plotWidth;
            float py = plotY + plotHeight * (1.0f - normY);

            screenPoints[i] = new Vector2(px, py);
        }

        Raylib.BeginScissorMode((int)plotX, (int)plotY, (int)plotWidth, (int)plotHeight);

        for (int i = 0; i < screenPoints.Length - 1; i++) 
        {
            Raylib.DrawLineEx(screenPoints[i], screenPoints[i + 1], 2.0f, neonCyan);
        }

        if (screenPoints.Length > 0) 
        {
            Raylib.DrawCircleV(screenPoints[^1], 3.5f, new Color(180, 255, 250, 255));
        }

        Raylib.EndScissorMode();
    }
}
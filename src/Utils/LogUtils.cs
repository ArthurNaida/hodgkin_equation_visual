using Raylib_cs;
using NeuroSimulation.Behaviors;
using NeuroSimulation.UI;

namespace NeuroSimulation.Utilities;

public static class LogUtils 
{
    public static void DrawLogWindow(int screenWidth, SimulationLogger logger) 
    {
        int boxWidth = 480;
        int boxHeight = 240;
        int boxX = screenWidth - boxWidth - 20;
        int boxY = 20;

        // Рисуем модальное окно через дизайн-систему UIWindow
        UIWindow.Draw(boxX, boxY, boxWidth, boxHeight, "HODGKIN-HUXLEY LIVE LOGS");

        int textY = boxY + 42;
        foreach (var log in logger.GetLogs()) 
        {
            Raylib.DrawText(log, boxX + 15, textY, 12, new Color(210, 190, 230, 255));
            textY += 26;
        }
    }
}
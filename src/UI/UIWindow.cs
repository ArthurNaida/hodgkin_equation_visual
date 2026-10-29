using Raylib_cs;

namespace NeuroSimulation.UI;

public static class UIWindow 
{
    public static void Draw(int x, int y, int width, int height, string title) 
    {
        // Полупрозрачная подложка панели
        Raylib.DrawRectangle(x, y, width, height, new Color(15, 10, 25, 220));
        Raylib.DrawRectangleLines(x, y, width, height, new Color(120, 90, 160, 255));

        // Шапка окна с заголовком
        Raylib.DrawText(title, x + 15, y + 12, 14, new Color(255, 220, 150, 255));
        Raylib.DrawLine(x + 15, y + 32, x + width - 15, y + 32, new Color(100, 70, 130, 200));
    }
}
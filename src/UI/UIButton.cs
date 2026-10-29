using System.Numerics;
using Raylib_cs;

namespace NeuroSimulation.UI;

public class UIButton 
{
    private readonly string label;

    public UIButton(string label) 
    {
        this.label = label;
    }

    public bool Draw(int x, int y, int width, int height) 
    {
        Vector2 mousePos = Raylib.GetMousePosition();
        Rectangle bounds = new Rectangle(x, y, width, height);
        
        bool isHovered = Raylib.CheckCollisionPointRec(mousePos, bounds);
        bool isClicked = isHovered && Raylib.IsMouseButtonPressed(MouseButton.Left);

        // Цветовая схема кнопки в стиле дизайн-системы
        Color bgColor = isHovered ? newCalculatorColor(100, 70, 150, 255) : new Color(60, 40, 90, 255);
        Color borderColor = new Color(150, 110, 200, 255);
        Color textColor = new Color(255, 240, 200, 255);

        Raylib.DrawRectangleRec(bounds, bgColor);
        Raylib.DrawRectangleLinesEx(bounds, 1, borderColor);

        // Центрируем текст внутри кнопки
        int fontSize = 12;
        int textWidth = Raylib.MeasureText(label, fontSize);
        int textX = x + (width - textWidth) / 2;
        int textY = y + (height - fontSize) / 2;

        Raylib.DrawText(label, textX, textY, fontSize, textColor);

        return isClicked;
    }

    private Color newCalculatorColor(int r, int g, int b, int a) => new Color(r, g, b, a);
}
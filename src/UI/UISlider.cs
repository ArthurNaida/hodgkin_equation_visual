using System.Numerics;
using Raylib_cs;

namespace NeuroSimulation.UI;

public class UISlider 
{
    private readonly string label;
    private float minValue;
    private float maxValue;
    private float currentValue;

    public UISlider(string label, float minValue, float maxValue, float initialValue) 
    {
        this.label = label;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.currentValue = initialValue;
    }

    public float Draw(int x, int y, int width, int height) 
    {
        // ИЗМЕНЕНИЕ: Формат F2 вместо F1 для точного отображения сотых долей (0.01, 0.02 и т.д.)
        Raylib.DrawText($"{label}: {currentValue:F2}", x, y, 12, new Color(210, 190, 230, 255));

        int sliderY = y + 18;
        int sliderHeight = 8;

        Raylib.DrawRectangle(x, sliderY, width, sliderHeight, new Color(50, 35, 70, 255));

        float normalizedValue = (currentValue - minValue) / (maxValue - minValue);
        int fillWidth = (int)(width * normalizedValue);
        Raylib.DrawRectangle(x, sliderY, fillWidth, sliderHeight, new Color(140, 100, 200, 255));

        Vector2 mousePos = Raylib.GetMousePosition();
        Rectangle bounds = new Rectangle(x, sliderY - 4, width, sliderHeight + 8);

        if (Raylib.IsMouseButtonDown(MouseButton.Left) && Raylib.CheckCollisionPointRec(mousePos, bounds)) 
        {
            float relX = mousePos.X - x;
            if (relX < 0) relX = 0;
            if (relX > width) relX = width;

            float t = relX / width;
            currentValue = minValue + t * (maxValue - minValue);
        }

        return currentValue;
    }
}
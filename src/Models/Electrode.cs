using System;
using System.Numerics;
using Raylib_cs;
using NeuroSimulation.UI;

namespace NeuroSimulation.Models;

public class Electrode 
{
    public Vector2 TipPosition; // Кончик электрода (прикоснут к соме)
    public Vector2 BasePosition; // Задняя часть электрода
    public bool IsStimulating { get; private set; }

    private readonly UIButton stimulateButton;
    private readonly float appliedCurrent;

    private int btnX;
    private int btnY;
    private readonly int btnWidth = 140;
    private readonly int btnHeight = 32;

    public Electrode(Vector2 somaCenter, float radiusX, float radiusY, float appliedCurrent = 30.0f) 
    {
        this.appliedCurrent = appliedCurrent;

        float angle = -MathF.PI * 0.25f;
        TipPosition = somaCenter + new Vector2(
            MathF.Cos(angle) * (radiusX * 0.95f),
            MathF.Sin(angle) * (radiusY * 0.95f)
        );

        Vector2 direction = Vector2.Normalize(TipPosition - somaCenter);
        BasePosition = TipPosition + direction * 100f;

        stimulateButton = new UIButton("STIMULATE");
    }

    public bool Update(Vector2 somaCenter, float radiusX, float radiusY, Camera2D camera) 
    {
        float angle = -MathF.PI * 0.25f;
        TipPosition = somaCenter + new Vector2(
            MathF.Cos(angle) * (radiusX * 0.92f),
            MathF.Sin(angle) * (radiusY * 0.92f)
        );
        Vector2 direction = Vector2.Normalize(TipPosition - somaCenter);

        BasePosition = TipPosition + direction * 100f;

        Vector2 worldButtonPos = BasePosition + new Vector2(10, -10);
        Vector2 screenButtonPos = Raylib.GetWorldToScreen2D(worldButtonPos, camera);

        btnX = (int)screenButtonPos.X;
        btnY = (int)screenButtonPos.Y;

        Vector2 mousePos = Raylib.GetMousePosition();
        Rectangle rect = new Rectangle(btnX, btnY, btnWidth, btnHeight);
        bool isHovered = Raylib.CheckCollisionPointRec(mousePos, rect);
        
        bool isMouseClicked = isHovered && Raylib.IsMouseButtonDown(MouseButton.Left);

        IsStimulating = isMouseClicked || Raylib.IsKeyDown(KeyboardKey.Space);
        return IsStimulating;
    }

    public void DrawWorld() 
    {
        Vector2 dir = Vector2.Normalize(BasePosition - TipPosition);
        Vector2 normal = new Vector2(-dir.Y, dir.X);

        float tipWidth = 3f;
        float baseWidth = 14f;

        Raylib.DrawLineEx(TipPosition, BasePosition, 2f, new Color(180, 200, 220, 200));

        Vector2 p1 = TipPosition + normal * (tipWidth * 0.5f);
        Vector2 p2 = TipPosition - normal * (tipWidth * 0.5f);
        Vector2 p3 = BasePosition - normal * (baseWidth * 0.5f);
        Vector2 p4 = BasePosition + normal * (baseWidth * 0.5f);

        Raylib.DrawSplineSegmentLinear(p1, p2, 1.5f, new Color(200, 230, 255, 180));
        Raylib.DrawSplineSegmentLinear(p2, p3, 1.5f, new Color(200, 230, 255, 120));
        Raylib.DrawSplineSegmentLinear(p3, p4, 1.5f, new Color(200, 230, 255, 180));
        Raylib.DrawSplineSegmentLinear(p4, p1, 1.5f, new Color(200, 230, 255, 120));

        if (IsStimulating) 
        {
            Raylib.DrawCircleV(TipPosition, 8f, new Color(0, 220, 255, 150));
            Raylib.DrawCircleV(TipPosition, 4f, new Color(220, 255, 255, 255));
        } 
        else 
        {
            Raylib.DrawCircleV(TipPosition, 3f, new Color(255, 140, 40, 180));
        }
    }

    public void DrawUI() 
    {
        stimulateButton.Draw(btnX, btnY, btnWidth, btnHeight);
    }
}
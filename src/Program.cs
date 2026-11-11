using Raylib_cs;
using NeuroSimulation.Simulation;

namespace NeuroSimulation;

class Program 
{
    static void Main(string[] args) 
    {
        Raylib.InitWindow(Context.ScreenWidth, Context.ScreenHeight, "Neuro Sim - Hodgkin-Huxley Design System HUD");
        Raylib.SetTargetFPS(60);

        var context = new Context();

        while (!Raylib.WindowShouldClose()) 
        {
            float frameDt = Raylib.GetFrameTime();

            // Шаг логики и физики
            Visual.Update(context, frameDt);

            // Шаг отрисовки
            Renderer.Draw(context);
        }

        Raylib.CloseWindow();
    }
}
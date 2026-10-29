using Raylib_cs;
using NeuroSimulation.Models;

namespace NeuroSimulation.UI;

public class PhysicsSettingsHud 
{
    private readonly UISlider gNaSlider = new UISlider("Max Na Conductance (GNa)", 0f, 240f, 120.0f);
    private readonly UISlider gKSlider = new UISlider("Max K Conductance (GK)", 0f, 100f, 36.0f);
    private readonly UISlider iExtSlider = new UISlider("External Current (I_ext)", 0f, 50f, 20.0f);
    
    // Академический диапазон dt: от 0.01 до 0.10 с дефолтом 0.01
    private readonly UISlider dtSlider = new UISlider("Integration Step (dt)", 0.01f, 0.10f, 0.01f);
    
    private readonly UIButton exportButton = new UIButton("Export Logs to File");

    public void Draw(int screenWidth, ref HodgkinHuxleyParameters p, ref double currentExternalCurrent, ref double currentDt, out bool exportRequested) 
    {
        int boxWidth = 340;
        int boxHeight = 260;
        int boxX = 20;
        int boxY = 60; 

        UIWindow.Draw(boxX, boxY, boxWidth, boxHeight, "PHYSICS HUD & CONTROLS");

        int startX = boxX + 15;
        int sliderWidth = boxWidth - 30;

        float newGNa = gNaSlider.Draw(startX, boxY + 40, sliderWidth, 18);
        float newGK = gKSlider.Draw(startX, boxY + 80, sliderWidth, 18);
        float newIExt = iExtSlider.Draw(startX, boxY + 120, sliderWidth, 18);
        float newDt = dtSlider.Draw(startX, boxY + 160, sliderWidth, 18);

        exportRequested = exportButton.Draw(startX, boxY + 200, sliderWidth, 30);

        currentExternalCurrent = (double)newIExt;
        currentDt = (double)newDt;
    }
}
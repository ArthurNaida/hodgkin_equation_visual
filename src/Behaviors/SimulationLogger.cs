using System.Collections.Generic;
using System.IO;

namespace NeuroSimulation.Behaviors;

public class SimulationLogger 
{
    private readonly Queue<string> logMessages = new Queue<string>();
    private readonly List<string> allLogsHistory = new List<string>();
    private const int MaxLines = 7;

    public void AddLog(double time, double voltage) 
    {
        string phaseDescription = GetVoltageDescription(voltage);
        string vText = (double.IsNaN(voltage) || double.IsInfinity(voltage)) ? "NaN" : voltage.ToString("F3");

        string logEntry = $"t: {time:F2}s | V: {vText,7} mV\n  -> {phaseDescription}";

        if (logMessages.Count >= MaxLines) 
        {
            logMessages.Dequeue();
        }
        logMessages.Enqueue(logEntry);
        
        allLogsHistory.Add($"t: {time:F2}s | V: {vText} mV | Status: {phaseDescription}");
    }

    public void ExportToFile(string filePath = "simulation_export.txt") 
    {
        File.WriteAllLines(filePath, allLogsHistory);
    }

    private string GetVoltageDescription(double v) 
    {
        if (double.IsNaN(v)) return "Error: Numerical Instability (NaN)";
        if (v <= -65.0) return "Resting State (Idle)";
        if (v > -65.0 && v < -50.0) return "Slow Depolarization";
        if (v >= -50.0 && v < -20.0) return "Threshold Reached!";
        if (v >= -20.0 && v < 20.0) return "Na+ Influx (Spike Start)";
        if (v >= 20.0) return "Action Potential Peak!";
        if (v < -70.0) return "Hyperpolarization (Refractory)";
        return "Repolarization (K+ Outflow)";
    }

    public IEnumerable<string> GetLogs() 
    {
        return logMessages;
    }
}
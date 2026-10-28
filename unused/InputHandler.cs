using System;

public static class InputHandler
{
    public static int GetTotalSteps(string[] args, int defaultSteps = 400)
    {
        foreach (string arg in args)
        {
            if (arg == "--no-input")
            {
                Console.WriteLine($"[Config] Flag --no-input detected. Using default totalSteps: {defaultSteps}");
                return defaultSteps;
            }
        }

        while (true)
        {
            Console.Write($"Enter total number of simulation steps (default {defaultSteps}, or enter custom): ");
            string input = Console.ReadLine();
            
            if (int.TryParse(input, out int steps) && steps > 0)
            {
                return steps;
            }
            Console.WriteLine("Invalid input. Please enter a positive integer.");
        }
    }
}

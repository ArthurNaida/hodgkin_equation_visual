/*using System;

// NEURON STATE STRUCTURE
// Stores only the mutable variables (membrane potential and gating variables)
public struct Neuron 
{
    public double V; // Membrane potential (mV)
    public double M; // Na activation gate
    public double H; // Na inactivation gate
    public double N; // K activation gate

    public Neuron(double v, double m, double h, double n) 
    {
        V = v;
        M = m;
        H = h;
        N = n;
    }
}

// MODEL PARAMETERS STRUCTURE
// Physical constants separated from calculations to avoid magic numbers
public struct HodgkinHuxleyParameters 
{
    public double Cm { get; } = 1.0;     // Membrane capacitance (uF/cm^2)
    public double GNa { get; } = 120.0;  // Max Na conductance (mS/cm^2)
    public double GK { get; } = 36.0;    // Max K conductance (mS/cm^2)
    public double GL { get; } = 0.3;     // Leak conductance (mS/cm^2)
    public double ENa { get; } = 50.0;   // Na reversal potential (mV)
    public double EK { get; } = -77.0;   // K reversal potential (mV)
    public double EL { get; } = -54.387; // Leak reversal potential (mV)

    public HodgkinHuxleyParameters() { }
}

class Program
{
    // Helper method to compute derivatives (rates of change) for any given state and current
    static (double dVdt, double dMdt, double dHdt, double dNdt) ComputeDerivatives(Neuron n, HodgkinHuxleyParameters p, double I_ext) 
    {
        // Kinetic rate functions (alpha and beta)
        double alphaM = 0.1 * (n.V + 40.0) / (1.0 - Math.Exp(-(n.V + 40.0) / 10.0));
        double betaM  = 4.0 * Math.Exp(-(n.V + 65.0) / 18.0);

        double alphaH = 0.07 * Math.Exp(-(n.V + 65.0) / 20.0);
        double betaH  = 1.0 / (1.0 + Math.Exp(-(n.V + 35.0) / 10.0));

        double alphaN = 0.01 * (n.V + 55.0) / (1.0 - Math.Exp(-(n.V + 55.0) / 10.0));
        double betaN  = 0.125 * Math.Exp(-(n.V + 65.0) / 80.0);

        // Ionic currents calculation
        double iNa = p.GNa * Math.Pow(n.M, 3) * n.H * (n.V - p.ENa);
        double iK  = p.GK  * Math.Pow(n.N, 4) * (n.V - p.EK);
        double iL  = p.GL  * (n.V - p.EL);

        // Derivatives calculation (rates of change)
        double dVdt = (I_ext - iNa - iK - iL) / p.Cm;
        double dMdt = alphaM * (1.0 - n.M) - betaM * n.M;
        double dHdt = alphaH * (1.0 - n.H) - betaH * n.H;
        double dNdt = alphaN * (1.0 - n.N) - betaN * n.N;

        return (dVdt, dMdt, dHdt, dNdt);
    }

    static void Main(string[] args)
    {
        // INITIALIZATION OF PARAMETERS AND STATE
        var p = new HodgkinHuxleyParameters(); // Model constants
        var neuron = new Neuron(-65.0, 0.05, 0.6, 0.32); // Initial resting state

        double dt = 0.01; // Time step (ms)
        int totalSteps = InputHandler.GetTotalSteps(args, defaultSteps: 800);

        Console.WriteLine("Starting Hodgkin-Huxley simulation with RK4...");

        // MAIN SIMULATION LOOP
        for (int i = 0; i < totalSteps; i++) 
        {
            double time = i * dt;

            // External current pulse applied from 1.0 to 3.0 ms
            double I_ext = (time >= 1.0 && time <= 3.0) ? 20.0 : 0.0;

            // --- RUNGE-KUTTA 4th ORDER INTEGRATION (RK4) ---
            // Step 1 (k1)
            var (k1_V, k1_M, k1_H, k1_N) = ComputeDerivatives(neuron, p, I_ext);

            // Step 2 (k2)
            var n2 = new Neuron(
                neuron.V + 0.5 * dt * k1_V,
                neuron.M + 0.5 * dt * k1_M,
                neuron.H + 0.5 * dt * k1_H,
                neuron.N + 0.5 * dt * k1_N
            );
            var (k2_V, k2_M, k2_H, k2_N) = ComputeDerivatives(n2, p, I_ext);

            // Step 3 (k3)
            var n3 = new Neuron(
                neuron.V + 0.5 * dt * k2_V,
                neuron.M + 0.5 * dt * k2_M,
                neuron.H + 0.5 * dt * k2_H,
                neuron.N + 0.5 * dt * k2_N
            );
            var (k3_V, k3_M, k3_H, k3_N) = ComputeDerivatives(n3, p, I_ext);

            // Step 4 (k4)
            var n4 = new Neuron(
                neuron.V + dt * k3_V,
                neuron.M + dt * k3_M,
                neuron.H + dt * k3_H,
                neuron.N + dt * k3_N
            );
            var (k4_V, k4_M, k4_H, k4_N) = ComputeDerivatives(n4, p, I_ext);

            // Final state update combining all steps with weighted average
            neuron.V += (dt / 6.0) * (k1_V + 2.0 * k2_V + 2.0 * k3_V + k4_V);
            neuron.M += (dt / 6.0) * (k1_M + 2.0 * k2_M + 2.0 * k3_M + k4_M);
            neuron.H += (dt / 6.0) * (k1_H + 2.0 * k2_H + 2.0 * k3_H + k4_H);
            neuron.N += (dt / 6.0) * (k1_N + 2.0 * k2_N + 2.0 * k3_N + k4_N);

            // Print output data to console
            Console.WriteLine($"t: {time,5:F2} ms | V: {neuron.V,6:F2} mV | I_ext: {I_ext}");
        }
    }
}*/
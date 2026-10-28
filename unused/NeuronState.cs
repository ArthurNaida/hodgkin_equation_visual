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
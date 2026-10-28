using System;

public static class HodgkinHuxleyModel 
{
    public static (double dVdt, double dMdt, double dHdt, double dNdt) ComputeDerivatives(Neuron n, HodgkinHuxleyParameters p, double I_ext) 
    {
        double alphaM = 0.1 * (n.V + 40.0) / (1.0 - Math.Exp(-(n.V + 40.0) / 10.0));
        double betaM  = 4.0 * Math.Exp(-(n.V + 65.0) / 18.0);

        double alphaH = 0.07 * Math.Exp(-(n.V + 65.0) / 20.0);
        double betaH  = 1.0 / (1.0 + Math.Exp(-(n.V + 35.0) / 10.0));

        double alphaN = 0.01 * (n.V + 55.0) / (1.0 - Math.Exp(-(n.V + 55.0) / 10.0));
        double betaN  = 0.125 * Math.Exp(-(n.V + 65.0) / 80.0);

        double iNa = p.GNa * Math.Pow(n.M, 3) * n.H * (n.V - p.ENa);
        double iK  = p.GK  * Math.Pow(n.N, 4) * (n.V - p.EK);
        double iL  = p.GL  * (n.V - p.EL);

        double dVdt = (I_ext - iNa - iK - iL) / p.Cm;
        double dMdt = alphaM * (1.0 - n.M) - betaM * n.M;
        double dHdt = alphaH * (1.0 - n.H) - betaH * n.H;
        double dNdt = alphaN * (1.0 - n.N) - betaN * n.N;

        return (dVdt, dMdt, dHdt, dNdt);
    }

    public static Neuron StepRK4(Neuron neuron, HodgkinHuxleyParameters p, double I_ext, double dt) 
    {
        var (k1_V, k1_M, k1_H, k1_N) = ComputeDerivatives(neuron, p, I_ext);

        var n2 = new Neuron(
            neuron.V + 0.5 * dt * k1_V,
            neuron.M + 0.5 * dt * k1_M,
            neuron.H + 0.5 * dt * k1_H,
            neuron.N + 0.5 * dt * k1_N
        );
        var (k2_V, k2_M, k2_H, k2_N) = ComputeDerivatives(n2, p, I_ext);

        var n3 = new Neuron(
            neuron.V + 0.5 * dt * k2_V,
            neuron.M + 0.5 * dt * k2_M,
            neuron.H + 0.5 * dt * k2_H,
            neuron.N + 0.5 * dt * k2_N
        );
        var (k3_V, k3_M, k3_H, k3_N) = ComputeDerivatives(n3, p, I_ext);

        var n4 = new Neuron(
            neuron.V + dt * k3_V,
            neuron.M + dt * k3_M,
            neuron.H + dt * k3_H,
            neuron.N + dt * k3_N
        );
        var (k4_V, k4_M, k4_H, k4_N) = ComputeDerivatives(n4, p, I_ext);

        double newV = neuron.V + (dt / 6.0) * (k1_V + 2.0 * k2_V + 2.0 * k3_V + k4_V);
        double newM = neuron.M + (dt / 6.0) * (k1_M + 2.0 * k2_M + 2.0 * k3_M + k4_M);
        double newH = neuron.H + (dt / 6.0) * (k1_H + 2.0 * k2_H + 2.0 * k3_H + k4_H);
        double newN = neuron.N + (dt / 6.0) * (k1_N + 2.0 * k2_N + 2.0 * k3_N + k4_N);

        return new Neuron(newV, newM, newH, newN);
    }
}
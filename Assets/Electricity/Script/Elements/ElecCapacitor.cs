using UnityEngine;

public class ElecCapacitor : ElecDipole
{
    public float capacitance = 0.01f; // en Farads
    public float storedCharge = 0.0f; // en Coulombs



    override public void IntensityToDeltaPotential()
    {
        sta.deltaPotential -= intensity * ElecManager.intensityToPotentialEffect;
        end.deltaPotential += intensity * ElecManager.intensityToPotentialEffect;
    }
    override public void ComputeIntensity()
    {
        float voltage = GetVoltage();
        float voltageInterne = storedCharge / capacitance;
        float diff = voltage - voltageInterne;

        intensity = capacitance * diff;
        storedCharge += intensity;
    }

    override public void ComputeDeltaPotential()
    {
        ComputeIntensity();
        IntensityToDeltaPotential();
    }
}

using UnityEngine;

public class ElecBattery : ElecDipole
{
    public double Voltage = 5.0f;
    override public void Init()
    {
        sta.potential = Voltage * 0.5f;
        end.potential = -Voltage * 0.5f;
    }

    override public void ComputeDeltaPotential()
    {
        intensity = GetVoltage() * ElecManager.conductanceWire;

        double v = GetVoltage();
        sta.deltaPotential += (Voltage - v) * 0.5f;
        end.deltaPotential += (v - Voltage) * 0.5f;
    }
}

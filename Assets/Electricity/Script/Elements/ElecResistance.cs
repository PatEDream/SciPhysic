using UnityEngine;

public class ElecResistance : ElecDipole
{
    public double ohm = 10.0f;
    override public void ComputeIntensity()
    {
        if (IsConnected())
        {
            double tension = ComputeVoltage();
            intensity = tension / ohm;
        }
    }

}

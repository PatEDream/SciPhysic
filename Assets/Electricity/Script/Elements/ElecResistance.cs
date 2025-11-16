using UnityEngine;

public class ElecResistance : ElecDipole
{
    public float ohm = 10.0f;
    override public void ComputeIntensity()
    {
        if (IsConnected())
        {
            float tension = GetVoltage();
            intensity = tension / ohm;

        }
    }

}

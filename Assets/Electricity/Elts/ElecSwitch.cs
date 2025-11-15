using UnityEngine;

public class ElecSwitch : ElecDipole
{
    public bool isOpen = true;


    override public void ComputeIntensity()
    {
        if (isOpen)
        {
            intensity = 0;
        }
        else
        {
            intensity = GetVoltage() * ElecManager.conductanceWire;
        }
    }



}
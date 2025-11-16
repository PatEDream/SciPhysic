using UnityEngine;

public class ElecSwitch : ElecDipole
{
    public bool isOpen = true;


    override public void Init()
    {
        name = "Switch";
    }

    override public string GetName()
    {
        return isOpen? "open" : "closed";
    }

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

    void OnMouseDown()
    {
        isOpen = !isOpen;
        Debug.Log($"{name} isOpen => {isOpen}");
    }

}
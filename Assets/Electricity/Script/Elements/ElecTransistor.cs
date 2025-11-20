using UnityEditor;
using UnityEngine;

public class ElecTransistor : ElecSwitch
{
    public ElecNode baseNode = new ElecNode();

    public float potentialThreshold = 0.7f;


    //override public void ComputeIntensity()
    //{
    //    isOpen = baseNode.potential < potentialThreshold;
    //    base.ComputeIntensity();
    //}

    //override public void IntensityToDeltaPotential()
    //{
    //    sta.deltaPotential -= intensity * ElecManager.intensityToPotentialEffect;
    //    end.deltaPotential += intensity * ElecManager.intensityToPotentialEffect;
    //}

    override public void ComputeDeltaPotential()
    {
        double VoltageBase = baseNode.potential - end.potential;
        double VoltageCut = VoltageBase < potentialThreshold ? VoltageBase : potentialThreshold;
        if (VoltageBase > 0.0 && VoltageCut > 0.0)
        {
            baseNode.deltaPotential -= VoltageCut * ElecManager.conductanceWire;
            end.deltaPotential -= VoltageCut * ElecManager.conductanceWire;
        }

        isOpen = (VoltageBase < potentialThreshold);

        base.ComputeDeltaPotential();
    }


    override protected void OnDrawGizmos()
    {
        base.OnDrawGizmos();

#if UNITY_EDITOR
        if (baseNode == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, baseNode.transform.position);
#endif
    }
}

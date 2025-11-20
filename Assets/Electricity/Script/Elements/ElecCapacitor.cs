using UnityEditor;
using UnityEngine;

public class ElecCapacitor : ElecDipole
{
    public double capacitance = 0.01f; // en Farads
    public double storedCharge = 0.0f; // en Coulombs



    override public void IntensityToDeltaPotential()
    {
        sta.deltaPotential -= intensity * ElecManager.intensityToPotentialEffect;
        end.deltaPotential += intensity * ElecManager.intensityToPotentialEffect;
    }
    override public void ComputeIntensity()
    {
        double voltage = ComputeVoltage();
        double voltageInterne = storedCharge / capacitance;
        double diff = voltage - voltageInterne;

        intensity = (capacitance * diff)/ ElecManager.intensityToPotentialEffect;
        storedCharge += intensity;
    }

    override public void ComputeDeltaPotential()
    {
        ComputeIntensity();
        IntensityToDeltaPotential();
    }


    override protected void OnDrawGizmos()
    {
        base.OnDrawGizmos();

#if UNITY_EDITOR
        Vector3 labelPos = transform.position + Vector3.up * 0.13f;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(labelPos, $"\n\n\nQ: {storedCharge:F2}", style);

#endif
    }
}

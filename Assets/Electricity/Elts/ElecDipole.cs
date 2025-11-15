using UnityEditor;
using UnityEngine;

public class ElecDipole : MonoBehaviour
{
    public ElecNode sta = new ElecNode();
    public ElecNode end = new ElecNode();

    public float intensity = 0.0f;

    protected void Start()
    {
        ElecManager.instance.dipoles.Add(this);
    }
    virtual public bool IsConnected()
    {
        return sta!=null && end != null;
    }

    virtual public float GetVoltage()
    {
        return sta.potential - end.potential;
    }

    virtual public void IntensityToDeltaPotential()
    {
        sta.deltaPotential -= intensity * ElecManager.intensityToPotentialEffect;
        end.deltaPotential += intensity * ElecManager.intensityToPotentialEffect;
    }
    virtual public void ComputeIntensity()
    {
    }

    virtual public void ComputeDeltaPotential()
    {
        ComputeIntensity();
        IntensityToDeltaPotential();
    }



    // Dessine en rouge une ligne Gizmo entre la position du dipôle (this.transform) et la position du node `sta`.
    void OnDrawGizmos()
    {
        if (sta == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, sta.transform.position);
        if (end == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, end.transform.position);

#if UNITY_EDITOR
        // label au-dessus du node : "V: <valeur>" avec 2 décimales
        float sphereRadius = 0.15f;
        Vector3 labelPos = transform.position + Vector3.up * (sphereRadius + 0.12f);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(labelPos, $"I: {intensity:F2}", style);
#endif
    }


}

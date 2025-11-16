using UnityEditor;
using UnityEngine;

public class ElecDipole : MonoBehaviour
{
    public ElecNode sta = new ElecNode();
    public ElecNode end = new ElecNode();

    public float intensity = 0.0f;

    protected void Start()
    {
        Init();
    }

    virtual public void Init()
    {
    }

    virtual public string GetName()
    {
        return gameObject.name;
    }

    virtual public bool IsConnected()
    {
        return sta != null && end != null;
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
        Vector3 labelPos = transform.position + Vector3.up * (sphereRadius + 0.0f);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(labelPos, $"{GetName()}\nI: {intensity:F2}\nV: {GetVoltage():F2}", style);
#endif
    }


}

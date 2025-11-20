#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ElecDipole : MonoBehaviour
{
    public ElecNode sta = new ElecNode();
    public ElecNode end = new ElecNode();

    public double intensity = 0.0f;

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

    virtual public double ComputeVoltage()
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
    virtual protected void OnDrawGizmos()
    {
        if (sta == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, sta.transform.position);
        if (end == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, end.transform.position);

        //#if UNITY_EDITOR
        Vector3 labelPos = transform.position + Vector3.up * 0.15f;// - Vector3.forward * 3.0f;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(labelPos, $"{GetName()}\nI: {intensity:F2}\nV: {ComputeVoltage():F2}", style);
#endif
    }

    


}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ElecNode : MonoBehaviour
{
    public float potential = 0.0f;
    public float deltaPotential = 0.0f;

    private void Start()
    {
        ElecManager.instance.nodes.Add(this);
        Init();
    }
    public void Init()
    {
        potential = 0.0f;
        deltaPotential = 0.0f;
    }

    public void UpdatePotential()
    {
        potential += deltaPotential;
        deltaPotential = 0.0f;
    }


    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // label au-dessus du node : "V: <valeur>" avec 2 décimales
        float sphereRadius = 0.05f;
        Vector3 labelPos = transform.position + Vector3.up * (sphereRadius + 0.12f);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(labelPos, $"V: {potential:F2}", style);
#endif
    }
}

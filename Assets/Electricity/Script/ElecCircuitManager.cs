using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ElecCircuitManager : MonoBehaviour
{
    public bool isRunning = false;

    public List<ElecDipole> dipoles;
    public List<ElecNode> nodes;


    public int nbPass = 100;


    public void Awake()
    {
        dipoles = GetComponentsInChildren<ElecDipole>(true).ToList();
        nodes = GetComponentsInChildren<ElecNode>(true).ToList();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        for (int i = 0; i < nbPass; i++)
        {
            foreach (ElecDipole dipole in dipoles)
            {
                if (dipole.isActiveAndEnabled)
                    dipole.ComputeDeltaPotential();
            }
            foreach (ElecNode node in nodes)
            {
                if (node.isActiveAndEnabled)
                    node.UpdatePotential();
            }
        }
    }

    public void ToggleRunning()
    {
        isRunning = !isRunning;
    }


    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Vector3 labelPos = transform.position;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(labelPos, isRunning ? "ON" : "OFF", style);
#endif
    }
}

using UnityEditor;
using UnityEngine;

public class ElecTransistor : ElecSwitch
{
    public ElecNode baseNode = new ElecNode();

    public float potentialThreshold = 0.7f;


    override public void ComputeIntensity()
    {
        isOpen = baseNode.potential < potentialThreshold;
        base.ComputeIntensity();
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

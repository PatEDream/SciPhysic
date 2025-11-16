using UnityEngine;

public class ElecBoard : MonoBehaviour
{
    ElecCircuitManager circuit;

    private void Awake()
    {
        circuit = GetComponentInParent<ElecCircuitManager>();
    }
    void OnMouseDown()
    {
        Vector3 pos = transform.position;
        pos.z = -10;
        Camera.main.transform.position = pos;
        //Debug.Log($"{name} selected");

        circuit.ToggleRunning();
    }


}

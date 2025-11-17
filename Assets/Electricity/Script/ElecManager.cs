using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ElecManager : MonoBehaviour
{
    public static ElecManager instance;
    public static double conductanceWire = 100f;
    public static double intensityToPotentialEffect = 0.001f;




    public int nbPass = 100;
    private void Awake()
    {
        instance = this;
    }


}

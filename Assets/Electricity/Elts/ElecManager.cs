using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ElecManager : MonoBehaviour
{
    public static ElecManager instance;
    public static float conductanceWire = 1000f;
    public static float intensityToPotentialEffect = 0.0001f;


    public List<ElecDipole> dipoles;
    public List<ElecNode> nodes;


    public int nbPass = 100;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        for (int i = 0; i < nbPass; i++)
        {
            foreach (ElecDipole dipole in dipoles)
            {
                dipole.ComputeDeltaPotential();
            }
            foreach (ElecNode node in nodes)
            {
                node.UpdatePotential();
            }
        }
    }
}

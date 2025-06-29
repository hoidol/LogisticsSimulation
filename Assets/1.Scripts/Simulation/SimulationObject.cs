using UnityEngine;
using System.Collections;

public class SimulationObject : MonoBehaviour
{
    public SimulationObjectType simulationObjectType;
}

public enum SimulationObjectType
{
    Machine,
    Box,
}

using UnityEngine;
using System.Collections;

public class SimulationObject : MonoBehaviour
{
    public SimulationObjectType simulationObjectType;
    public FocusIndicator focusIndicator;

    private void Awake()
    {
        if (focusIndicator == null)
            focusIndicator = GetComponentInChildren<FocusIndicator>();

        focusIndicator.gameObject.SetActive(false);
    }

    public virtual void Focus(bool on)
    {
        focusIndicator.gameObject.SetActive(on);
    }

    public virtual void ViewDetail(bool on)
    {

    }
}

public enum SimulationObjectType
{
    Machine,
    Box,
}

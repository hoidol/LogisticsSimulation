using UnityEngine;

public abstract class SimulationMode : MonoBehaviour
{
    public SimulationModeType type;

    public abstract void StartMode();
    public abstract void EndMode();
}

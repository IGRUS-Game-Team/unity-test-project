using UnityEngine;

public class MonitorInteractionManager : MonoBehaviour
{
    public static MonitorInteractionManager Instace { get; private set; }

    public bool IsActive => isMonitorInteractionManagerPlaying;
    private bool isMonitorInteractionManagerPlaying;
}

using UnityEngine;

public class KioskInteractionManager : MonoBehaviour
{
    public static KioskInteractionManager Instace { get; private set; }

    public bool IsActive => isKioskInteractionManagerPlaying;
    private bool isKioskInteractionManagerPlaying;
}

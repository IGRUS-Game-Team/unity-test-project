using UnityEngine;

public class KioskInteractionManager : MonoBehaviour
{
    public static KioskInteractionManager Instance { get; private set; }

    public bool IsActive => isKioskInteractionManagerPlaying;
    private bool isKioskInteractionManagerPlaying;
}

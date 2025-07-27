using UnityEngine;

//KioskInteractionManager.cs 박정민
//미구현

public class KioskInteractionManager : MonoBehaviour
{
    public static KioskInteractionManager Instance { get; private set; }

    public bool IsActive => isKioskInteractionManagerPlaying;
    private bool isKioskInteractionManagerPlaying;
}

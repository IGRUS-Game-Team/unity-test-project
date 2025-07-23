using UnityEngine;

public class BoxInteractionManager : MonoBehaviour
{
    public static BoxInteractionManager Instace { get; private set; }

    public bool IsActive => isBoxInteractionManagerPlaying;
    private bool isBoxInteractionManagerPlaying;
}

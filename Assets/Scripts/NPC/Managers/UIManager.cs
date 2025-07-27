using UnityEngine;
using TMPro;          // TextMeshPro 사용 예

public class UIManager : MonoBehaviour
{
    public static UIManager I { get; private set; }

    [SerializeField] GameObject payPanel;        // “E - 돈 받기” 패널
    [SerializeField] TextMeshProUGUI npcNameTxt;

    void Awake() => I = this;

    public void ShowPayPrompt(NpcController npc)
    {
        npcNameTxt.text = npc.name;
        payPanel.SetActive(true);
    }

    public void HidePayPrompt()
    {
        payPanel.SetActive(false);
    }
}
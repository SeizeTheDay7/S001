using UnityEngine;
using TMPro;

public class Debug : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI debugText;
    [SerializeField] PlayerMove playerMove;

    void Update()
    {
        // bool isClicking = playerMove.isClicking;
        // string debug_1 = "isClicking : ";
        // debug_1 += isClicking ? "Clicking" : "Not Clicking";

        // debugText.text = $"{debug_1}";
    }
}
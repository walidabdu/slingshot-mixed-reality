using UnityEngine;
using TMPro;

public class VRKeypad3D : MonoBehaviour
{
    [Header("3D TextMeshPro Target")]
    public TextMeshPro textDisplay;

    [Header("Max Characters")]
    public int maxCharacters = 10;

    private string currentText = "";

    // Called by number buttons: pass "0"..."9"
    public void PressNumber(string number)
    {
        if (textDisplay == null || string.IsNullOrEmpty(number)) return;

        if (currentText.Length < maxCharacters)
        {
            currentText += number;
            UpdateDisplay();
        }
    }

    // Called by the "Clear" button
    public void PressClear()
    {
        currentText = "";
        UpdateDisplay();
    }

    // Called by the "Backspace" button
    public void PressBackspace()
    {
        if (currentText.Length > 0)
        {
            currentText = currentText.Substring(0, currentText.Length - 1);
            UpdateDisplay();
        }
    }

    // Called by the "OK" or "Accept" button
    public void PressOK()
    {
        Debug.Log("Accepted Value: " + currentText);
        // TODO: Submit logic goes here
    }

    private void UpdateDisplay()
    {
        textDisplay.text = currentText;
    }
}

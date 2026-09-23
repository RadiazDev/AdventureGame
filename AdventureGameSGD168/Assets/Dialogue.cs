using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    private int openedFrame;
    [SerializeField] GameObject buttonBlocker;

    // Restart whenever the box opens, including repeat conversations.
    private void OnEnable()
    {
        if (textComponent == null || lines == null || lines.Length == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        openedFrame = Time.frameCount;
        index = 0;
        textComponent.text = "";
        if (buttonBlocker != null) buttonBlocker.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (buttonBlocker != null) buttonBlocker.SetActive(false);
    }

    public void Show(string[] newLines)
    {
        gameObject.SetActive(false);
        lines = newLines;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // The click that opens the box must not also skip its first line.
        if (Time.frameCount != openedFrame && Input.GetMouseButtonDown(0)) Advance();
    }

    public void Advance()
    {
        if (!gameObject.activeInHierarchy) return;
        if (textComponent.text != lines[index])
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
        else if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = "";
            StartCoroutine(TypeLine());
        }
        else gameObject.SetActive(false);
    }

    private IEnumerator TypeLine()
    {
        foreach (char letter in lines[index])
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}

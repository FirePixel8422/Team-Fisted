using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Use this if you're using Unity's UI Text
using TMPro; // Uncomment this if you're using TextMesh Pro

public class LetterByLetter : MonoBehaviour
{
    public float typingSpeed = 0.1f; // Time in seconds between each letter
    public string fullText; // The full text to display
    private string currentText = ""; // The text currently displayed

    //public Text textComponent; // For UI Text
    public TMP_Text textComponent; // For TextMesh Pro
    public GameObject[] audioSource;

    private void Start()
    {
        //textComponent = GetComponent<TMP_Text>(); // For TextMesh Pro
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            PlaySound(); // Play audio when a new letter appear
            currentText += letter; // Append each letter
            textComponent.text = currentText; // Update the text component
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified time
        }
    }

    public void PlaySound()
    {
        Instantiate(audioSource[Random.Range(0, audioSource.Length)], transform);
    }
}
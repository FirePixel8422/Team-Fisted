using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Use this if you're using Unity's UI Text
using TMPro; // Uncomment this if you're using TextMesh Pro

public class LetterByLetter : MonoBehaviour
{
    public float typingSpeed = 0.1f; // Time in seconds between each letter
    public string fullText; // The full text to display
    private string currentText = ""; // The text currently displayed

    public float minPitch, maxPitch;

    //public Text textComponent; // For UI Text
    public TMP_Text textComponent; // For TextMesh Pro
    public AudioSource audioSource; // For AudioSource
    public AudioClip audioClip; // For AudioClip

    private void OnEnable()
    {
        currentText = "";

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        float elapsed = 0;

        foreach (char letter in fullText.ToCharArray())
        {
            PlaySound(); // Play audio when a new letter appear
            currentText += letter; // Append each letter
            textComponent.text = currentText; // Update the text component


            while (elapsed < typingSpeed)
            {
                yield return null;

                elapsed += Time.deltaTime;
            }
            elapsed -= typingSpeed;
        }
    }

    public void PlaySound()
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(audioClip); // Play audioclip
    }
}
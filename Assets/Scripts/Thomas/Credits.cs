using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public TextMeshProUGUI textObj;

    public float creditSwapInterval;

    public StringWithEnters[] credits;


    private int creditId;



    private void Start()
    {
        StartCoroutine(CreditSwapTimer());
    }
    public IEnumerator CreditSwapTimer()
    {
        WaitForSeconds wait = new WaitForSeconds(creditSwapInterval);
        while (true)
        {
            yield return wait;

            textObj.text = credits[creditId].GetStringAsText();


            creditId += 1;
            if (creditId == credits.Length)
            {
                creditId = 0;
            }
        }
    }


    [System.Serializable]
    public struct StringWithEnters
    {
        public String[] textRules;


        public string GetStringAsText()
        {
            string newString = "";

            for (int i = 0; i < textRules.Length; i++)
            {
                newString += textRules[i].text + (textRules[i].addEnter ? "\n" : "");
            }

            return newString;
        }
        
        
        [System.Serializable]
        public struct String
        {
            public string text;
            public bool addEnter;
        }
    }
}

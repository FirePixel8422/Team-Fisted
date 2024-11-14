using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorTicking : MonoBehaviour
{
    public TextMesh textObj;

    private int textIndex;
    public string[] textStrings;

    public float[] dotDotDotTimes;





    private void Start()
    {
        StartCoroutine(TickLoop());
    }

    private IEnumerator TickLoop()
    {
        WaitForSeconds[] waitTimes = new WaitForSeconds[dotDotDotTimes.Length];

        for (int i = 0; i < waitTimes.Length; i++)
        {
            waitTimes[i] = new WaitForSeconds(dotDotDotTimes[i]);
        }


        while (true)
        {
            yield return waitTimes[textIndex];

            textIndex += 1;
            if (textIndex == textStrings.Length)
            {
                textIndex = 0;
            }

            textObj.text = textStrings[textIndex];
        }
    }
}

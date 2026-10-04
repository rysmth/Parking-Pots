using System.Collections;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    public float countdownTime;
    private bool isCountingDown = true;
    public GameObject canvas;
    public GameObject management;

    void Update()
    {
        if (isCountingDown)
        {
            countdownTime -= Time.deltaTime;
            int seconds = Mathf.RoundToInt(countdownTime);

            if (seconds <= 0)
            {
                isCountingDown = false;
                StartCoroutine(HideCountdownDisplay());
            }
        }
    }

    IEnumerator HideCountdownDisplay()
    {
        yield return new WaitForSeconds(1f);
        canvas.gameObject.SetActive(false);
        management.gameObject.SetActive(true);
    }
}
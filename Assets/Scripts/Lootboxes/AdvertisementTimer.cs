using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class AdvertisementTimer : MonoBehaviour
{
    [SerializeField] private float timerLength;
    [SerializeField] private TextMeshProUGUI timerText;
    public UnityEvent onCompleted, onCancelled;

    private float startTime;
    private float timeLeft => timerLength - (Time.time - startTime);

    private void OnEnable()
    {
        startTime = Time.time;
    }

    private void OnDisable()
    {
        if (timeLeft > 0)
            Cancel();
    }

    private void Update()
    {
        print(timeLeft);
        timerText.text = Mathf.Floor(timeLeft).ToString();
        if (timeLeft <= 0)
        {
            onCompleted?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void Cancel()
    {
        StopAllCoroutines();
        onCancelled?.Invoke();
    }
}

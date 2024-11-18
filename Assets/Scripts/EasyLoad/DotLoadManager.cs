using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public partial class DotLoadManager : MonoBehaviour, ILoading
{
    private static DotLoadManager instance;
    private static readonly object lockObj = new();
    private Coroutine maxLoad;
    private float startPositionY;
    public static DotLoadManager Instance => instance;

    private void Awake()
    {
        if (!instance)
        {
            lock (lockObj)
            {
                if(!instance) instance = this;
            }
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        loadUiCanvas.SetActive(false);
        
        if (repeatTime < dotObject.Length * bounceTime) repeatTime = dotObject.Length * bounceTime;
        startPositionY = dotObject[0].transform.localPosition.y;
    }

    public void Loading(bool load)
    {
        if (load)
        {
            OnInvoke();
            if (maxLoad != null) StopCoroutine(maxLoad);
            maxLoad = StartCoroutine(MaxLoadTime());
        }
        else
        {
            OnCancelInvoke();
            if (maxLoad != null)
            {
                StopCoroutine(maxLoad);
                maxLoad = null;
            }
        }

        foreach (var dot in dotObject) 
            dot.transform.localPosition = new Vector3(
                dot.transform.localPosition.x, 
                startPositionY, 
                dot.transform.localPosition.z);
    }

    private IEnumerator MaxLoadTime()
    {
        yield return new WaitForSeconds(maxTime);
        OnCancelInvoke();
        maxLoad = null;
    }

    private void OnInvoke()
    {
        InvokeRepeating(nameof(OnDotLoading), 0, repeatTime);
        loadUiCanvas.SetActive(true);
    }

    private void OnCancelInvoke()
    {
        CancelInvoke(nameof(OnDotLoading));
        loadUiCanvas.SetActive(false);
    }

    private void OnDotLoading()
    {
        for (int i = 0; i < dotObject.Length; i++)
        {
            int localI = i;
            dotObject[localI].transform
                .DOLocalMoveY(dotObject[localI].transform.localPosition.y + bounceHeight, bounceTime / 2)
                .SetDelay(localI * bounceTime / 2)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    dotObject[localI].transform
                        .DOLocalMoveY(dotObject[localI].transform.localPosition.y - bounceHeight, bounceTime / 2)
                        .SetEase(Ease.InQuad);
                });
        }
    }
}

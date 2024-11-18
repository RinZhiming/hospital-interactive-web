using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerExtension : MonoBehaviour
{
    private static SceneManagerExtension instance;
    private static readonly object lockObj = new();
    private Coroutine maxLoad;
    
    public static SceneManagerExtension Instance => instance;

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
    }
    
    /// <summary>
    /// Easy load scene for my project always with Instance. 
    /// </summary>
    /// <param name="scene">You can add more scene in <see cref="SceneName"/> enum.</param>
    public void LoadScene(SceneName scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }
    
    private IEnumerator LoadSceneAsync(SceneName scene)
    {
        var load = SceneManager.LoadSceneAsync(scene.ToString());
        if (load != null)
        {
            if (!DotLoadManager.Instance)
            {
                Debug.Log($"Null Ref loading");
                yield break;
            }

            DotLoadManager.Instance.Loading(true);

            yield return new WaitUntil(() => load.isDone);
        }
        
        DotLoadManager.Instance.Loading(false);
    }
}

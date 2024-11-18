using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public partial class CharacterSelectManager : MonoBehaviour
{
    private void Awake()
    {
        CharacterIndex = 0;
    }

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        selectButton.onClick.AddListener(OnSelectButtonClicked);

        contain.localPosition = new Vector3(-200f, contain.localPosition.y, contain.localPosition.z);

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void OnSelectButtonClicked()
    {
        var data = new { iconProfile = CharacterIndex + 6 };
        var iconProfile = JsonConvert.SerializeObject(data);
        var uid = PlayerPrefs.GetString("uid");
        var path = $"doctors/{uid}/";
        ApiConnector.PatchDatabase(iconProfile, path,HttpCaller.Token, e => Error(e, "PatchDatabase"), PatchDatabaseComplete);
    }

    private void PatchDatabaseComplete(string obj)
    {
        var uid = PlayerPrefs.GetString("uid");
        var path = $"doctors/{uid}";
        ApiConnector.GetDatabase(path,HttpCaller.Token,e => Error(e, "GetDatabase"), GetDatabaseComplete);
    }

    private void GetDatabaseComplete(string obj)
    {
        PlayerPrefs.SetString("profile", obj);
        PlayerPrefs.Save();
        
        var profile = JsonUtility.FromJson<DoctorData>(obj);
        CharacterIndex = profile.iconProfile;
        
        SceneManagerExtension.Instance.LoadScene(SceneName.HomeScene);
    }

    private void Error(HttpErrorCode obj, string s)
    {
        Debug.Log(s + obj.ToString());
    }

    private void OnNextButtonClicked()
    {
        if (contain.localPosition.x <= -2700f) return;
        if (!onSlide)
        { 
            StartCoroutine(ScrollDelay(contain.localPosition.x, contain.localPosition.x - 500f));
            CharacterIndex++;
        }
    }

    private void OnBackButtonClicked()
    {
        if (contain.localPosition.x > -300f) return;
        if (!onSlide)
        { 
            StartCoroutine(ScrollDelay(contain.localPosition.x, contain.localPosition.x + 500f));
            CharacterIndex--;
        }
    }

    private IEnumerator ScrollDelay(float start, float end)
    {
        onSlide = true;
        DOVirtual.Float(start, end, scrollSpeed, f =>
        {
            contain.localPosition = new Vector3(f, contain.localPosition.y, contain.localPosition.z);
        });
        yield return new WaitForSeconds(scrollSpeed + 0.1f);
        onSlide = false;
    }
}

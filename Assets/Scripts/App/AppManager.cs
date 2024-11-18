using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    [SerializeField] private SceneName loginScene, homeScene;
    [SerializeField] private bool forgetUser;

    private void Awake()
    {
        if(forgetUser) PlayerPrefs.DeleteKey("uid");
    }

    private void Start()
    {
        AutoLogin();
    }

    private string GetUid()
    {
        var getuid = PlayerPrefs.GetString("uid");
        return getuid;
    }

    private void AutoLogin()
    {
        if (string.IsNullOrEmpty(GetUid()))
        {
            SceneManagerExtension.Instance.LoadScene(SceneName.LoginScene);
            return;
        }
        ApiConnector.AutoLogin(GetUid(), Error, Complete );
    }

    private void Complete(string obj)
    {
        var data = PlayerPrefs.GetString("profile");
        var profile = JsonUtility.FromJson<DoctorData>(data);
        var rawData = JObject.Parse(obj);
        var token = rawData["token"]?.ToString();
        HttpCaller.Token = token;
        CharacterSelectManager.CharacterIndex = profile.iconProfile;
        SceneManagerExtension.Instance.LoadScene(profile.iconProfile == -1
            ? SceneName.CharacterSelectScene
            : SceneName.HomeScene);
    }

    private void Error(HttpErrorCode obj)
    {
        Debug.LogError(obj.ToString());
        SceneManager.LoadScene(loginScene.ToString());
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class HttpDecoder
{
    /// <summary>
    /// Decode from <c>string</c> to <c>JObject</c>
    /// </summary>
    /// <param name="msg">string</param>
    /// <returns></returns>
    public static JObject Decode(string msg)
    {
        try
        {
            var data = JObject.Parse(msg);
            return data;
        }
        catch (JsonException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public static T Decode<T>(string data)
    {
        var obj = JsonUtility.FromJson<T>(data);
        return obj;
    }
}
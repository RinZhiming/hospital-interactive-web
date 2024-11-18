using UnityEditor;

[CustomEditor(typeof(ApiConfiguration))]
public class ApiConfigurationEditor : Editor
{
    private SerializedProperty serverType, serverAddress, serverPort;
    private void OnEnable()
    {
        serverType = serializedObject.FindProperty("serverType");
        serverAddress = serializedObject.FindProperty("serverAddress");
        serverPort = serializedObject.FindProperty("serverPort");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        var config = (ApiConfiguration)target;
        EditorGUILayout.PropertyField(serverType);
        switch (config.ServerType)
        {
            case Server.Port:
                EditorGUILayout.PropertyField(serverPort);
                config.ServerAddress = string.Empty;
                break;
            case Server.Url:
                EditorGUILayout.PropertyField(serverAddress);
                config.ServerPort = default;
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
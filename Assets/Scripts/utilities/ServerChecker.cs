using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ServerChecker
{
    public event Action<bool,string> OnCheckerFinish;
    public const string GOOGLE = "http://www.google.com";
    public const string IPV4_REGEX = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
    
    private readonly MonoBehaviour monoBehaviour;

    public ServerChecker(MonoBehaviour monoBehaviour)
    {
        this.monoBehaviour = monoBehaviour;
    }

    public void CheckWithGoogle()
    {
        monoBehaviour.StartCoroutine(Check(GOOGLE));
    }
    
    private IEnumerator Check(string server)
    {
        string message = "something is wrong with your internet conection, please check it. ";
        bool result = false;
        UnityWebRequest request = new UnityWebRequest(server);

        yield return request.SendWebRequest();

        result = !request.isNetworkError && !request.isHttpError;

        if (result)
            message = "";
        if (OnCheckerFinish != null)
            OnCheckerFinish(result, message+request.error);

        monoBehaviour.StopCoroutine(Check(server));
    }
}

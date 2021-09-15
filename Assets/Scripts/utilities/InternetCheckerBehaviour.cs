using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetCheckerBehaviour : MonoBehaviour {

    
    public ServerChecker ServerChecker { get; private set; }

    private void Awake()
    {
        ServerChecker = new ServerChecker(this);
    }

    public void CheckWithGoogle()
    {
        ServerChecker.CheckWithGoogle();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineDirector : MonoBehaviour
{
    
    public void NextScene()
    {
        AudioManagement.Instance.LoadNextScene();
    }


}

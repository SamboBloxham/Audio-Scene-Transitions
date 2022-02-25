using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroupedCredits : MonoBehaviour
{

    [SerializeField]
    string[] creditsContent;

    int nameGroupIndex = 0;

    [SerializeField]
    TMP_Text creditsText;


    [SerializeField]
    Animator anim;




    float fadeLength = 2;
    
    [SerializeField]
    float groupDisplayLength = 4;



    public void CallStartCredits()
    {
        StartCoroutine(StartCredits());
    }


    IEnumerator StartCredits()
    {
        creditsText.text = creditsContent[nameGroupIndex].Replace("\\n", "\n");
        anim.SetTrigger("FadeIn");

        yield return Wait(groupDisplayLength + fadeLength);
        StartCoroutine(NextGroup());
    }

    
    IEnumerator NextGroup()
    {
        anim.SetTrigger("FadeOut");

        yield return Wait(fadeLength);

        nameGroupIndex++;
        creditsText.text = creditsContent[nameGroupIndex].Replace("\\n", "\n");

        anim.SetTrigger("FadeIn");


        yield return Wait(groupDisplayLength + fadeLength);


        if(nameGroupIndex >= creditsContent.Length - 1)
        {
            CreditsEnd();
        }
        else
        {
            StartCoroutine(NextGroup());
        }
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }



    void CreditsEnd()
    {
        anim.SetTrigger("FadeOut");
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSceneManager : MonoBehaviour
{


    [SerializeField]
    Animator animController;

    Transform inSceneCamera;
    Transform inSceneXROrigin;

    public static TransitionSceneManager Instance;


    [SerializeField]
    TitlecardUpdater titleCard;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        FadeOut();

    }

    public void FadeOut()
    {
        animController.SetTrigger("fadeout");
    }

    public void FadeIn()
    {


        titleCard.DeactivateTitleCard();
        animController.SetTrigger("fadein");

    }



}

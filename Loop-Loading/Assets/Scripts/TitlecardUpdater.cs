using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitlecardUpdater : MonoBehaviour
{

    [SerializeField]
    public TitleCard[] titleCards;

    [SerializeField]
    TMP_Text titleText;

    [SerializeField]
    GameObject titleCardObject;


    [SerializeField]
    float titleCardShowDelay = 2f;

    static int titleIterator = 0;


    TitlecardUpdater Instance;

    void Awake()
    {
        Instance = this;

        UpdateTitleCardVariables();

        titleIterator++;
        titleCardObject.SetActive(false);
        Invoke("ActivateTitleCard", titleCardShowDelay);
    }
    

    void UpdateTitleCardVariables()
    {
        //Public strings "\n" are saved as "\\n" for some reason
        print(titleIterator + "Titerator");
        titleText.text = titleCards[titleIterator].titleName.Replace("\\n", "\n");
        titleText.color = titleCards[titleIterator].colour;
    }


    public void ActivateTitleCard()
    {
        titleCardObject.SetActive(true);
    }
    public void DeactivateTitleCard()
    {
        titleCardObject.SetActive(false);
    }

    [System.Serializable]
    public class TitleCard
    {
        public string titleName;
        public Color colour;
    }

}

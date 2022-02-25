using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Timeline;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AudioManagement : MonoBehaviour
{
    

    [SerializeField]
    AudioSource[] transitionTracks;




    [SerializeField]
    int sceneStartPosition = 1;


    bool startSceneAllowed = false;

    [SerializeField]
    float minimumTransitionLength = 0f;


    int mainTrackPosition = 0;
    int transitionTrackPosition = 0;

    int scenePosition;


    bool sceneCurrentlyLoading = true;


    bool autoLoadNextScenes = false;

    [SerializeField]
    public PlayableDirector timeline;


    int timelinePosition = -1;


    public static AudioManagement Instance;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Instance = this;

        scenePosition = sceneStartPosition;

        //mainTracks[mainTrackPosition].Play();


        //autoLoadNextScenes = true;
        
    }


    public void LoadFirstScene()
    {
        
        autoLoadNextScenes = true;
        StartCoroutine(TransitionToNextSceneInArray());
    }
    

    void Update()
    {
        //if(autoLoadNextScenes && !sceneCurrentlyLoading)
        //if (CheckSongIsAlmostFinished(mainTracks[mainTrackPosition]))
        //{

        //    if (mainTrackPosition != mainTracks.Length - 1)
        //    {
        //            sceneCurrentlyLoading = true;
        //            StartCoroutine(TransitionToNextSceneInArray());
        //    }
        //}

    }


    bool CheckSongIsAlmostFinished(AudioSource track)
    {
        return track.time >= (track.clip.length - 1f);
    }


    public void LoadNextScene()
    {
        print("loadnextscene");
        sceneCurrentlyLoading = true;
        StartCoroutine(TransitionToNextSceneInArray());
    }

    IEnumerator TransitionToNextSceneInArray()
    {

        LoadTransition();


        yield return StartCoroutine(StartLoadNextScene(scenePosition, transitionTrackPosition));


        timelinePosition++;

        yield return StartCoroutine(BeginNextScene(scenePosition, timelinePosition));

        scenePosition++;
        transitionTrackPosition++;
    }



    public void TransitionToScene(int newSceneBuildIndex, int transitionTrack, int mainTrack)
    {
        StartCoroutine(TransitionToSpecificScene(newSceneBuildIndex, transitionTrack, mainTrack));
    }

    public void TransitionToScene(string newSceneName, int transitionTrack, int mainTrack)
    {
        int newSceneBuildIndex = SceneManager.GetSceneByName(newSceneName).buildIndex;

        StartCoroutine(TransitionToSpecificScene(newSceneBuildIndex, transitionTrack, mainTrack));
    }


    IEnumerator TransitionToSpecificScene(int newSceneBuildIndex, int transitionTrack, int mainTrack)
    {

        LoadTransition();

        yield return StartCoroutine(StartLoadNextScene(newSceneBuildIndex, transitionTrack));

        yield return StartCoroutine(BeginNextScene(newSceneBuildIndex, mainTrack));

    }


    




    void LoadTransition()
    {
        sceneCurrentlyLoading = true;
        Invoke("PlayTransitionAudio", 1f);
        SceneManager.LoadScene("Transition Scene", LoadSceneMode.Additive);
    }

    void PlayTransitionAudio()
    {
        transitionTracks[transitionTrackPosition].Play();
    }

    AsyncOperation loadNextScene;
    AsyncOperation unloadPreviousScene;


    //Loads the next scene in the build index and unloads the previous one
    IEnumerator StartLoadNextScene(int newSceneBuildIndex, int transitionTrack)
    {

        yield return new WaitForSeconds(2f);


        

        unloadPreviousScene = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());


        while (!unloadPreviousScene.isDone)
        {
            if (!transitionTracks[transitionTrack].isPlaying)
            {
                transitionTracks[transitionTrack].Play();
            }

            yield return null;
        }


        timeline = null;


        loadNextScene = SceneManager.LoadSceneAsync(newSceneBuildIndex, LoadSceneMode.Additive);


        //loadNextScene.allowSceneActivation = false; 

        //while (loadNextScene.progress !>= 0.9)
        //{ 
        //    if(!transitionTracks[transitionTrack].isPlaying)
        //    {
        //        transitionTracks[transitionTrack].Play();
        //    }

        //    yield return null;
        //}

        //loadNextScene.allowSceneActivation = true;

        StartCoroutine(AllowSceneStart(minimumTransitionLength));


        while (!loadNextScene.isDone || !startSceneAllowed)
        {
            if (!transitionTracks[transitionTrack].isPlaying)
            {
                transitionTracks[transitionTrack].Play();
            }

            yield return null;
        }

        print("startscene" + startSceneAllowed + minimumTransitionLength);


        while (transitionTracks[transitionTrack].isPlaying)
        {

            print("trans track" + transitionTrack);
            yield return null;
        }

    }


    IEnumerator AllowSceneStart(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        startSceneAllowed = true;
    }



    //Officially starts the next scene - Plays the scene's music and unloads the transition scene
    IEnumerator BeginNextScene(int newSceneBuildIndex, int nextTimeline)
    {

        print("NEW SCENE");

        timeline = GameObject.Find("Timeline").GetComponent<PlayableDirector>();

        timeline.Play();



        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(newSceneBuildIndex));

        TransitionSceneManager.Instance.FadeIn();

        startSceneAllowed = false;

        Invoke("UnloadTransition", 2f);

        yield return null;
        
    }

    void UnloadTransition()
    {

        SceneManager.UnloadSceneAsync("Transition Scene");
        
        sceneCurrentlyLoading = false;

    }



}

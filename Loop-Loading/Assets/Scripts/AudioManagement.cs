using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagement : MonoBehaviour
{
    
    [SerializeField]
    AudioSource[] mainTracks;

    [SerializeField]
    AudioSource[] transitionTracks;



    [SerializeField]
    int sceneStartPosition = 1;

    

    int mainTrackPosition = 0;
    int transitionTrackPosition = 0;

    int scenePosition;


    bool sceneCurrentlyLoading = true;


    bool autoLoadNextScenes = false;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        scenePosition = sceneStartPosition;

        mainTracks[mainTrackPosition].Play();

    }


    public void LoadFirstScene()
    {
        autoLoadNextScenes = true;
        StartCoroutine(TransitionToNextSceneInArray());
    }
    

    void Update()
    {
        if(autoLoadNextScenes && !sceneCurrentlyLoading)
        if (CheckSongIsAlmostFinished(mainTracks[mainTrackPosition]))
        {

            if (mainTrackPosition != mainTracks.Length - 1)
            {
                    sceneCurrentlyLoading = true;
                    StartCoroutine(TransitionToNextSceneInArray());
            }
        }
    }


    bool CheckSongIsAlmostFinished(AudioSource track)
    {
        return track.time >= (track.clip.length - 1f);
    }


    IEnumerator TransitionToNextSceneInArray()
    {

        LoadTransition();


        yield return StartCoroutine(StartLoadNextScene(scenePosition, transitionTrackPosition));


        mainTrackPosition++;

        yield return StartCoroutine(BeginNextScene(scenePosition, mainTrackPosition));

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


        
        loadNextScene = SceneManager.LoadSceneAsync(newSceneBuildIndex, LoadSceneMode.Additive);

        unloadPreviousScene = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());


        loadNextScene.allowSceneActivation = false;



        while (loadNextScene.progress !>= 0.9)
        { 
            if(!transitionTracks[transitionTrack].isPlaying)
            {
                transitionTracks[transitionTrack].Play();
            }

            yield return null;
        }



        while (transitionTracks[transitionTrack].isPlaying)
        {
            yield return null;
        }



    }

    //Officially starts the next scene - Plays the scene's music and unloads the transition scene
    IEnumerator BeginNextScene(int newSceneBuildIndex, int nextMainTrack)
    {

        loadNextScene.allowSceneActivation = true;

        mainTracks[nextMainTrack].Play();

        while (!loadNextScene.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(newSceneBuildIndex));

        

        TransitionSceneManager.Instance.FadeIn();

        Invoke("UnloadTransition", 2f);
        
    }

    void UnloadTransition()
    {

        SceneManager.UnloadSceneAsync("Transition Scene");
        
        sceneCurrentlyLoading = false;

    }



}

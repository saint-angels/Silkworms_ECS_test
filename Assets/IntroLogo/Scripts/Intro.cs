using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] private PlayableDirector timelineDirector = null;
    [SerializeField] private AudioClip titleShowClip = null;
    [SerializeField] private AudioSource audioSource = null;

    [SerializeField] private bool playOnAwake = true;


    // Start is called before the first frame update
    void Awake()
    {
        if (playOnAwake)
        {
            StartCoroutine(StartTimeline());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StartTimeline());
        }
    }

    private IEnumerator StartTimeline()
    {
        yield return new WaitForSeconds(1f);
        timelineDirector.Play();
        Invoke("TransitionToNextScene", (float)timelineDirector.duration);
        PlayTitleShowClip();
    }

    private void TransitionToNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void PlayTitleShowClip()
    {
        audioSource.PlayOneShot(titleShowClip);
    }
}

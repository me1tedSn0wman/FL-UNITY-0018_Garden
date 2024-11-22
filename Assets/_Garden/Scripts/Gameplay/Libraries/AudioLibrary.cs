using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AudioClipData {
    public string title;
    public AudioClip audioClip;
}

[RequireComponent(typeof(AudioControl))]
public class AudioLibrary : MonoBehaviour
{
    [SerializeField] private List<AudioClipData> listOfAudioClips;

    private Dictionary<string, AudioClip> dictOfAudioClips;
    private AudioControl audioControl;
    public void Awake()
    {
        audioControl = GetComponent<AudioControl>();
        InitDictionary();
    }

    private void InitDictionary() {
        dictOfAudioClips= new Dictionary<string, AudioClip>();
        for (int i = 0; i < listOfAudioClips.Count; i++) {
            dictOfAudioClips.Add(listOfAudioClips[i].title, listOfAudioClips[i].audioClip);
        }
    }

    private AudioClip FindClipByName(string title) {
        if (!dictOfAudioClips.ContainsKey(title)) {

            Debug.Log("(AudioLibrary) There are no audioClip with that title: " + title);
            return null;
        }
        return dictOfAudioClips[title]; 
    }

    public void PlayOneShoot(string title) {
        AudioClip audioClip= FindClipByName(title);
        audioControl.PlayOneShot(audioClip);
    }

    public void PlayLoop(string title) { 
        AudioClip audioClip = FindClipByName(title);
        audioControl.PlayLoop(audioClip);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public GameObject mosqEncounterVoice;
    public GameObject flippedNarration;

    bool mosqEncounterVoiceFlag = true;
    bool flippedNarrationFlag = true;

    void Start()
    {
        mosqEncounterVoice.SetActive(false);
        flippedNarration.SetActive(false);
    }

    void Update()
    {
        if(SetOfMosqSet.gameStarted && mosqEncounterVoiceFlag)
        {
            mosqEncounterVoice.SetActive(true);
            mosqEncounterVoiceFlag = false;
        }

        if(LastSceneText.isFlipped && flippedNarrationFlag)
        {
            flippedNarration.SetActive(true);
            flippedNarrationFlag = false;
        }
    }
}

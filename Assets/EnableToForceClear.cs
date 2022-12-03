using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableToForceClear : MonoBehaviour
{
    public GameObject esg;
    private void OnEnable()
    {
        esg.GetComponent<EnableToForceStart>().enabled = true;
        SetOfMosqSet.gameCleared = true;
    }
}

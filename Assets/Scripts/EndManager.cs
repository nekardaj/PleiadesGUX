using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public GameObject good;
    public GameObject bad;

    public bool goodOrBad;

    public void SetEndPicture()
    {
        if (goodOrBad)
        {
            good.SetActive(true);
        }
        else
        {
            bad.SetActive(true);
        }
    }
}

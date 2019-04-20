using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : SingletonAsComponent<GlobalSettings>
{
    public GameObject GameLogicManager;
    public int MaxPlayerCard = 5;
    public int DefaultPlayerScore = 0;

    public static GlobalSettings Instance
    {
        get { return ((GlobalSettings)_Instance); }
        set { _Instance = value; }
    }
}

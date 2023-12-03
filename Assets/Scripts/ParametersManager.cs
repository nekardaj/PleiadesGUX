using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametersManager : MonoBehaviour
{
    [SerializeField]
    private ParameterSettings[] parameterSettings;
    private int version = 0;

    public static ParametersManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public ParameterSettings GetParameterSettings()
    {
        return parameterSettings[version];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVersion(int version)
    {
        this.version = version;
        Debug.Log("Version set to " + version);
    }
}

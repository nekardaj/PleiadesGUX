using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using TMPro;
public class ParametersManager : MonoBehaviour
{
    [SerializeField]
    private VariantType[] parameterSettings;
    private static VariantType variant;
    [SerializeField]
    private TMP_Dropdown Dropdown;

    public static ParametersManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // Get dropdown component and set it to the value saved in variant variable
        //Dropdown dropdown = GetComponent<Dropdown>();
        //dropdown.value = parameterSettings.ToList().IndexOf(variant);
        Dropdown.value = parameterSettings.ToList().IndexOf(variant);
    }

    public static VariantType GetParameterSettings()
    {
        return variant;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVersion(int version)
    {
        variant = parameterSettings[version];
        Debug.Log("Version set to " + version);
    }
}

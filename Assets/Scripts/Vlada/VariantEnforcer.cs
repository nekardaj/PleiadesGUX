using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VariantEnforcer : MonoBehaviour
{
    // Singleton instance
    private VariantType _variantType;
    public VariantType CurrentVariantType { get { return _variantType; } }

    [SerializeField] private FlockMovement _flockMovement;
    [SerializeField] private FlockManager _flockManager;
    [SerializeField] private SO_Variant _slowVariant;
    [SerializeField] private SO_Variant _fastVariant;
    [SerializeField] private TextMeshProUGUI _variantText;

    private SO_Variant _variant;

    public static VariantEnforcer Instance;

    private void Awake()
    {
        Instance = this;
        //// Ensure only one instance exists
        //if (Instance != null && Instance != this)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    Instance = this;
        //    //DontDestroyOnLoad(this.gameObject);
        //}
    }

    private void Start()
    {
        SetVariant(ParametersManager.GetParameterSettings());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SetSlowVariant();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SetFastVariant();
        }
    }

    public void SetVariant(VariantType type)
    {
        switch (type)
        {
            case VariantType.Slow:
                SetSlowVariant();
                break;
            case VariantType.Fast:
                SetFastVariant();
                break;
            default:
                Debug.LogError("[VariantEnforcer] No Variant Type set!");
                SetSlowVariant();
                break;
        }
    }

    public void SetSlowVariant()
    {
        _variant = _slowVariant;
        _variantType = VariantType.Slow;
        EnforceVariant();
        _variantText.text = "variant 1";
        Debug.Log("SLOW VARIANT");
    }

    public void SetFastVariant()
    {
        _variant = _fastVariant;
        _variantType = VariantType.Fast;
        EnforceVariant();
        _variantText.text = "variant 2";
        Debug.Log("FAST VARIANT");
    }

    private void EnforceVariant()
    {
        _flockMovement.movementSpeed = _variant.FlockSpeed;
        _flockMovement.ReactionSpeed = _variant.FlockRotationSpeed;
        _flockMovement.DecaySpeed = _variant.FlockReturnSpeed;
        _flockMovement.MaxRotation = _variant.MaxAngle;

        _flockManager.StarReward = _variant.StarReward;
    }
}

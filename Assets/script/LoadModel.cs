using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriLibCore.General;
using TriLibCore.Extensions;
using UnityEngine.UI;
using TriLibCore;
using UnityEngine.UIElements;


public class LoadModel : MonoBehaviour
{
    /// <summary>
    /// The last loaded GameObject.
    /// </summary>
    private GameObject _loadedGameObject;

    /// <summary>
    /// Wrapper object to attach downloaded model.
    /// </summary>
    public GameObject _wrapperObj;

    /// <summary>
    /// File selection panel Gameobject
    /// </summary>
    [SerializeField]
    private GameObject _closeLoadButton;

    /// <summary>
    /// Progress panel Gameobject
    /// </summary>
    [SerializeField]
    private GameObject _progressPanel;

    /// <summary>
    /// Progress text Gameobject
    /// </summary>
    [SerializeField]
    private Text _progressText;

    private Vector3 _actualmodelSize;
    private Vector3 _actualmodelRot;
    
    /// <summary>
    /// Sets the app orientation to Potrait
    /// </summary>
    public void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    /// <summary>
    /// Creates the AssetLoaderOptions instance and displays the Model file-picker.
    /// Activate progress panel
    /// </summary>
    public void LoadModelfile()
    {
        var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        assetLoaderOptions.AnimationWrapMode = WrapMode.Once;
        var assetLoaderFilePicker = AssetLoaderFilePicker.Create();
        assetLoaderFilePicker.LoadModelFromFilePickerAsync("Select a Model file", OnLoad, OnMaterialsLoad, OnProgress, null, OnError, _wrapperObj, assetLoaderOptions);
        _progressPanel.SetActive(true);
    }

    /// <summary>
    /// Called when any error occurs.
    /// </summary>
    private void OnError(IContextualizedError obj)
    {
        Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
    }

    /// <summary>
    /// Called when the Model loading progress changes.
    /// Closes the overall file selection ui
    /// </summary>
    private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
    {
        _progressText.text = $"Progress: {progress:P}";
        float prgvalue = progress;
        if (prgvalue > 0.9f)
        {
            _closeLoadButton.SetActive(false);
        }
    }

    /// <summary>
    /// Called when the Model (including Textures and Materials) has been fully loaded.
    /// </summary>
    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        if (assetLoaderContext.RootGameObject != null)
        {
            Debug.Log("Model fully loaded.");
        }
        else
        {
            Debug.Log("Model could not be loaded.");
        }
    }

    /// <summary>
    /// Called when the Model Meshes and hierarchy are loaded.
    /// Added boxcollider,referenced loaded gameobject and enabled  touch script
    /// Added tag and changed the app screen orientation to landscape
    /// </summary>
    private void OnLoad(AssetLoaderContext assetLoaderContext)
    {
        _loadedGameObject = assetLoaderContext.RootGameObject;
        BoxCollider _bc = _loadedGameObject.AddComponent<BoxCollider>();
        _bc.size = new Vector3(6, 6, 6);
        TouchScript ts = this.GetComponent<TouchScript>();
        ts.modeltarget = _loadedGameObject.transform;
        _loadedGameObject.tag = "ArObj";
        if (!ts.enabled)
        {
            ts.enabled = true;
        }
        
        _wrapperObj.SetActive(true);
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        _actualmodelSize = _loadedGameObject.transform.localScale;
        _actualmodelRot = _loadedGameObject.transform.localEulerAngles;
    }
    /// <summary>
    /// Called when the reset button is clicked.
    /// rerest the position the 3dmodel
    /// </summary>
    public void ResetModel()
    {
        _loadedGameObject.transform.localScale = _actualmodelSize;
        _loadedGameObject.transform.localEulerAngles = _actualmodelRot;
    }

    /// <summary>
    /// Called when the close button is clicked.
    /// Closes the app
    /// </summary>
    public void QuitmyApp()
    {
        Application.Quit();
    }
}

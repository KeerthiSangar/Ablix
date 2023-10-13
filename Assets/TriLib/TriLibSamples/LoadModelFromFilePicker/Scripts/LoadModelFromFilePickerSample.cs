#pragma warning disable 649
using TriLibCore.General;
using UnityEngine;
using TriLibCore.Extensions;
using UnityEngine.UI;

namespace TriLibCore.Samples
{
    /// <summary>
    /// Represents a sample that loads a Model from a file-picker.
    /// </summary>
    public class LoadModelFromFilePickerSample : MonoBehaviour
    {
        /// <summary>
        /// The last loaded GameObject.
        /// </summary>
        private GameObject _loadedGameObject;

        public GameObject _wrapperObj;
        /// <summary>
        /// The load Model Button.
        /// </summary>
        [SerializeField]
        private Button _loadModelButton;

        /// <summary>
        /// The progress indicator Text;
        /// </summary>
        [SerializeField]
        private Text _progressText;

        /// <summary>
        /// Creates the AssetLoaderOptions instance and displays the Model file-picker.
        /// </summary>
        /// <remarks>
        /// You can create the AssetLoaderOptions by right clicking on the Assets Explorer and selecting "TriLib->Create->AssetLoaderOptions->Pre-Built AssetLoaderOptions".
        /// </remarks>
        public void LoadModel()
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            var assetLoaderFilePicker = AssetLoaderFilePicker.Create();
            assetLoaderFilePicker.LoadModelFromFilePickerAsync("Select a Model file", OnLoad, OnMaterialsLoad, OnProgress, null, OnError, _wrapperObj, assetLoaderOptions);
        }

        /// <summary>
        /// Called when any error occurs.
        /// </summary>
        /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
        private void OnError(IContextualizedError obj)
        {
            Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
        }

        /// <summary>
        /// Called when the Model loading progress changes.
        /// </summary>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        /// <param name="progress">The loading progress.</param>
        private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            _progressText.text = $"Progress: {progress:P}";
        }

        /// <summary>
        /// Called when the Model (including Textures and Materials) has been fully loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
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
            _loadModelButton.interactable = true;
            _progressText.enabled = false;
        }

        /// <summary>
        /// Called when the Model Meshes and hierarchy are loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
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
            //if (_loadedGameObject != null)
            //{
            //    Destroy(_loadedGameObject);
            //}
            //
            //if (_loadedGameObject != null)
            //{
            //   // _loadedGameObject.transform.localScale = Vector3.one * 0.1f;
            //}
        }
    }
}

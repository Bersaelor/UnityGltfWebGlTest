using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using GLTFast;
using GLTFast.Loading;

public class ModelDownloader : MonoBehaviour
{

    [Tooltip("URL to load the glTF from.")]
    public string url;

    // private GltfAsset gltfAsset;

    void Awake()
    {
        // gltfAsset = gameObject.AddComponent<GltfAsset>();
    }


    // Start is called before the first frame update
    void Start()
    {
        string basePath = Application.persistentDataPath; 
        string localFilePath = Path.Combine(basePath, Path.GetFileName(url));
        StartCoroutine(DownloadFile(url, data => loadGLTF(data)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DownloadFile(string url, System.Action<byte[]> callback) {
        using (var www = UnityWebRequest.Get(url)) {
            var dh = new DownloadHandlerBuffer();
            www.downloadHandler = dh;
            Debug.Log("[ModelDownloader] Downloading file from " + url);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log("[ModelDownloader] Failed downloading " + url + " because of " + www.error);
            } else if (www.responseCode != (long)System.Net.HttpStatusCode.OK) {
                Debug.Log("[ModelDownloader] Failed downloading " + url + " with status code " + www.responseCode);
            } else {
                callback(dh.data);
            }
        }
    }

    private async void loadGLTF(byte[] data) {
        Debug.Log("[ModelDownloader]: Loading file bytes of size " + data.Length);
        try {
            // var fileProvider = new LocalFileProvider();
            var gltf = new GLTFast.GLTFast();
            var success = await gltf.Load(data);

            gltf.InstantiateGltf(transform);

            if (!success) {
                Debug.LogWarning("[ModelDownloader]: failed to load model: ");
            }
        } catch (System.Exception e) {
            Debug.LogWarning("[ModelDownloader]: Error while loading model: " + e.Message);
            throw e;
        }
    }
}

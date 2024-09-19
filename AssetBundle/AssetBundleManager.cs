using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AssetBundleManager : MonoBehaviour
{
    public static AssetBundleManager I;private void Awake(){I = this;}

    public static Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();

    public Image image;


    private void Start()
    {
        //var bundle=LoadAssetBundle("gameassets");
        //var bundle = LoadAssetBundleFromNetwork("https://github.com/Moe1024/UnityToolAssets/raw/main/gameassets");

        //image.sprite = bundle.LoadAsset<Sprite>("image");

        //StartCoroutine(Upload_Delay());
    }

    IEnumerator Upload_Delay()
    {
        yield return new WaitForSeconds(2f);
        UploadAssetBundle("gameassets");
    }


    public static AssetBundle LoadAssetBundle(string assetBundleName)
    {
        string bundlePath = "Assets/StreamingAssets/" + assetBundleName;
        if (!loadedAssetBundles.ContainsKey(assetBundleName))
            loadedAssetBundles.Add(assetBundleName, AssetBundle.LoadFromFile(bundlePath));
        else
            loadedAssetBundles[assetBundleName] = AssetBundle.LoadFromFile(bundlePath);

        return loadedAssetBundles[assetBundleName];
    }

    public AssetBundle LoadAssetBundleFromNetwork(string url)
    {
        AssetBundle bundle = null;
        StartCoroutine(DownloadFromNetwork(bundle, url));
        return bundle;

    }
    IEnumerator DownloadFromNetwork(AssetBundle bundle, string url)
    {

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                bundle = DownloadHandlerAssetBundle.GetContent(www);
                Debug.Log("www.result: " + www.result);
                Debug.Log("bundle.name: " + bundle.name);

                if (!loadedAssetBundles.ContainsKey(bundle.name))
                    loadedAssetBundles.Add(bundle.name, bundle);
                else
                    loadedAssetBundles[bundle.name] = bundle;


                //image.sprite = bundle.LoadAsset<Sprite>("image");
                //StartCoroutine(Upload_Delay());

            }
            else
            {
                Debug.LogError("Error downloading Asset Bundle: " + www.error);
            }
            www.Dispose();
        }

    }



    public static T LoadAsset<T>(string assetBundleName, string assetPath) where T : Object
    {
        return loadedAssetBundles[assetBundleName].LoadAsset<T>(assetPath);
    }

    public static void UploadAssetBundle(string assetBundleName)
    {
        loadedAssetBundles[assetBundleName].Unload(false);
    }
    public static void WholeUploadAssetBundle(string assetBundleName)
    {
        loadedAssetBundles[assetBundleName].Unload(true);
    }
}

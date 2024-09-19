using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Reflection;
using UnityEngine.Networking;

public class HybirdHotUpdateManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadDLL("https://github.com/Moe1024/UnityToolAssets/raw/main/HotUpdate.dll.bytes", "HotUpdate.dll.bytes"));

        //CallHotUpdateFun();

    }

    private void CallHotUpdateFun()
    {
        //HotUpdate需要先打成Assembly按程序集并添加进Project setting里的Hot Update Assembly Definitions里
        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdate.dll.bytes"));

        Type type = hotUpdateAss.GetType("TestHotUpdateNamespace.TestHotUpdate");
        type.GetMethod("Hello").Invoke(null, null);

    }


    IEnumerator DownloadDLL(string url,string dllName)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // 获取DLL文件的字节数据
            byte[] dllData = www.downloadHandler.data;

            // 获取StreamingAssets文件夹的路径
            string streamingAssetsPath = Application.streamingAssetsPath;
            string dllPath = Path.Combine(streamingAssetsPath, dllName);

            // 保存DLL文件到StreamingAssets文件夹
            File.WriteAllBytes(dllPath, dllData);

            CallHotUpdateFun();
        }
        else
        {
            Debug.LogError("Download error: " + www.error);
        }
    }



}

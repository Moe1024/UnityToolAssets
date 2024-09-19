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
        //HotUpdate��Ҫ�ȴ��Assembly�����򼯲���ӽ�Project setting���Hot Update Assembly Definitions��
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
            // ��ȡDLL�ļ����ֽ�����
            byte[] dllData = www.downloadHandler.data;

            // ��ȡStreamingAssets�ļ��е�·��
            string streamingAssetsPath = Application.streamingAssetsPath;
            string dllPath = Path.Combine(streamingAssetsPath, dllName);

            // ����DLL�ļ���StreamingAssets�ļ���
            File.WriteAllBytes(dllPath, dllData);

            CallHotUpdateFun();
        }
        else
        {
            Debug.LogError("Download error: " + www.error);
        }
    }



}

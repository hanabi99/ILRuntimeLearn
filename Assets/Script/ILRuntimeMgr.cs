using ILRuntime.Mono.Cecil.Pdb;
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ILRuntimeMgr : MonoBehaviour
{
    private static ILRuntimeMgr instance;

    public AppDomain appDomain;
    //用于存储加载出来的 两个文件的内存流对象
    private MemoryStream dllStream;
    private MemoryStream pdbStream;

    private bool isStart = false;

    public static ILRuntimeMgr Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("ILRuntimeMgr");
                instance = obj.AddComponent<ILRuntimeMgr>();
                DontDestroyOnLoad(obj);
            }

            return instance;
        }
    }

    /// <summary>
    /// 用于启动ILRuntime初始化方法
    /// </summary>
    public void StartILRuntime( UnityAction callBack = null )
    {
        if(!isStart)
        {
            isStart = true;
            appDomain = new AppDomain();
            StartCoroutine(LoadHotUpdateInfo(callBack));
        }
    }

    public void StopILRuntime()
    {
        //1.释放流
        if (dllStream != null)
            dllStream.Close();
        if (pdbStream != null)
            pdbStream.Close();
        dllStream = null;
        pdbStream = null;
        //2.清空appDomain
        appDomain = null;

        isStart = false;
    }

    /// <summary>
    /// 初始化ILRuntime相关的内容
    /// </summary>
    private void InitILRuntime()
    {
        //其他初始化

        //初始化ILRuntime相关信息（目前只需要告诉ILRuntime主线程的线程ID，主要目的是能够在Unity的Profiler剖析器窗口中分析问题）
        appDomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
    }

    /// <summary>
    /// 启动完毕并且初始化完毕后 想要执行的热更新的逻辑
    /// </summary>
    private void ILRuntimeLoadOverDoSomthing()
    {

    }

    /// <summary>
    /// 去异步加载我们的热更相关的dll和pdb文件
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadHotUpdateInfo(UnityAction callBack)
    {
        //这部分知识点在 Unity网络开发基础当中有讲解
        //加载本地的DLL文件
#if UNITY_ANDROID
        UnityWebRequest reqDll = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix_Project.dll");
#else
        UnityWebRequest reqDll = UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/HotFix_Project.dll");
#endif
        yield return reqDll.SendWebRequest();
        //如果失败了
        if (reqDll.result != UnityWebRequest.Result.Success)
            print("加载DLL文件失败" + reqDll.responseCode + reqDll.result);
        //读取加载的DLL数据
        byte[] dll = reqDll.downloadHandler.data;
        reqDll.Dispose();

#if UNITY_ANDROID
        UnityWebRequest reqPdb = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix_Project.pdb");
#else
        UnityWebRequest reqPdb = UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/HotFix_Project.pdb");
#endif
        yield return reqPdb.SendWebRequest();
        //如果失败了
        if (reqPdb.result != UnityWebRequest.Result.Success)
            print("加载Pdb文件失败" + reqPdb.responseCode + reqPdb.result);
        //读取加载的DLL数据
        byte[] pdb = reqPdb.downloadHandler.data;
        reqPdb.Dispose();

        //3.将加载的数据以流的形式(文件流或者内存流对象)传递给AppDomain对象中的LoadAssembly方法
        //注意 这里使用流 不要用完就关 一定要等到热更相关内容不使用了 再关闭
        dllStream = new MemoryStream(dll);
        pdbStream = new MemoryStream(pdb);
        //将我们两个文件的内存流用于初始化 appDomain 我们之后就可以通过该对象来执行我们对应的热更代码了
        appDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());

        InitILRuntime();

        ILRuntimeLoadOverDoSomthing();

        //当ILRuntime启动完毕 想要在外部执行的内容
        callBack?.Invoke();
    }
}

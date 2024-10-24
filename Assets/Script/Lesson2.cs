using ILRuntime.Mono.Cecil.Pdb;
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson2 : MonoBehaviour
{
    private AppDomain appDomain;

    //用于存储加载出来的 两个文件的内存流对象
    private MemoryStream dllStream;
    private MemoryStream pdbStream;

    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 ILRuntime关键类AppDomain
        //上节课，我们知道ILRuntime的开发方式
        //是在Unity主工程和ILRuntime热更工程中进行开发的
        //两个工程之间可以相互访问调用

        //ILRuntime热更代码最终会生成一个dll文件和一个pdb文件
        //这里面就包含了我们热更代码的相关信息

        //而ILRuntime提供了AppDomain类
        //是ILRuntime提供的用于解释执行dll和pdb文件的
        //通过它我们才能解释执行我们的热更代码

        //它的作用就有点类似xLua中的LuaEnv lua解析器
        #endregion

        #region 知识点二 Unity中启动ILRuntime
        //启动方式
        //1.声明AppDomain对象(命名空间 ILRuntime.Runtime.Enviorment)
        appDomain = new AppDomain();
        //2.加载本地或远端下载的dll和pdb文件
        StartCoroutine(LoadHotUpdateInfo());
        //3.将加载的数据以流的形式(文件流或者内存流对象)传递给AppDomain对象中的LoadAssembly方法

        //4.初始化ILRuntime相关信息（目前只需要告诉ILRuntime主线程的线程ID，主要目的是能够在Unity的Profiler剖析器窗口中分析问题）

        //5.执行热更代码中的逻辑

        //注意：
        //一般在一个项目中，大多数情况下只需要一个AppDomain对象
        #endregion

        #region 总结
        //从本节Unity中启动ILRuntime我们更能够感受到
        //ILRuntime热更新的内容其实就是热更工程中的dll文件和pdb文件
        //这两个文件中就包含了我们所有的热更C#代码信息
        //以后如果我们要做远端热更新，只需要把他们放入AB包下载即可
        #endregion
    }

    /// <summary>
    /// 去异步加载我们的热更相关的dll和pdb文件
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadHotUpdateInfo()
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
        //流对象不能用完关闭 必须等到不需要调用热更逻辑才能关闭
        //将我们两个文件的内存流用于初始化 appDomain 我们之后就可以通过该对象来执行我们对应的热更代码了
        appDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());

        //4.初始化ILRuntime相关信息（目前只需要告诉ILRuntime主线程的线程ID，主要目的是能够在Unity的Profiler剖析器窗口中分析问题）
        appDomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;


        //5.执行热更代码中的逻辑
        print("我们已经对ILRuntime初始化成功 之后就可以执行对应逻辑了");
    }

    private void OnDestroy()
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
    }
}

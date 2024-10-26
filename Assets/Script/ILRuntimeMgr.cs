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
    //���ڴ洢���س����� �����ļ����ڴ�������
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
    /// ��������ILRuntime��ʼ������
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
        //1.�ͷ���
        if (dllStream != null)
            dllStream.Close();
        if (pdbStream != null)
            pdbStream.Close();
        dllStream = null;
        pdbStream = null;
        //2.���appDomain
        appDomain = null;

        isStart = false;
    }

    /// <summary>
    /// ��ʼ��ILRuntime��ص�����
    /// </summary>
    private void InitILRuntime()
    {
        //������ʼ��

        //��ʼ��ILRuntime�����Ϣ��Ŀǰֻ��Ҫ����ILRuntime���̵߳��߳�ID����ҪĿ�����ܹ���Unity��Profiler�����������з������⣩
        appDomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
    }

    /// <summary>
    /// ������ϲ��ҳ�ʼ����Ϻ� ��Ҫִ�е��ȸ��µ��߼�
    /// </summary>
    private void ILRuntimeLoadOverDoSomthing()
    {

    }

    /// <summary>
    /// ȥ�첽�������ǵ��ȸ���ص�dll��pdb�ļ�
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadHotUpdateInfo(UnityAction callBack)
    {
        //�ⲿ��֪ʶ���� Unity���翪�����������н���
        //���ر��ص�DLL�ļ�
#if UNITY_ANDROID
        UnityWebRequest reqDll = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix_Project.dll");
#else
        UnityWebRequest reqDll = UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/HotFix_Project.dll");
#endif
        yield return reqDll.SendWebRequest();
        //���ʧ����
        if (reqDll.result != UnityWebRequest.Result.Success)
            print("����DLL�ļ�ʧ��" + reqDll.responseCode + reqDll.result);
        //��ȡ���ص�DLL����
        byte[] dll = reqDll.downloadHandler.data;
        reqDll.Dispose();

#if UNITY_ANDROID
        UnityWebRequest reqPdb = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix_Project.pdb");
#else
        UnityWebRequest reqPdb = UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/HotFix_Project.pdb");
#endif
        yield return reqPdb.SendWebRequest();
        //���ʧ����
        if (reqPdb.result != UnityWebRequest.Result.Success)
            print("����Pdb�ļ�ʧ��" + reqPdb.responseCode + reqPdb.result);
        //��ȡ���ص�DLL����
        byte[] pdb = reqPdb.downloadHandler.data;
        reqPdb.Dispose();

        //3.�����ص�������������ʽ(�ļ��������ڴ�������)���ݸ�AppDomain�����е�LoadAssembly����
        //ע�� ����ʹ���� ��Ҫ����͹� һ��Ҫ�ȵ��ȸ�������ݲ�ʹ���� �ٹر�
        dllStream = new MemoryStream(dll);
        pdbStream = new MemoryStream(pdb);
        //�����������ļ����ڴ������ڳ�ʼ�� appDomain ����֮��Ϳ���ͨ���ö�����ִ�����Ƕ�Ӧ���ȸ�������
        appDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());

        InitILRuntime();

        ILRuntimeLoadOverDoSomthing();

        //��ILRuntime������� ��Ҫ���ⲿִ�е�����
        callBack?.Invoke();
    }
}

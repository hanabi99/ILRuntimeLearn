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

    //���ڴ洢���س����� �����ļ����ڴ�������
    private MemoryStream dllStream;
    private MemoryStream pdbStream;

    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ ILRuntime�ؼ���AppDomain
        //�ϽڿΣ�����֪��ILRuntime�Ŀ�����ʽ
        //����Unity�����̺�ILRuntime�ȸ������н��п�����
        //��������֮������໥���ʵ���

        //ILRuntime�ȸ��������ջ�����һ��dll�ļ���һ��pdb�ļ�
        //������Ͱ����������ȸ�����������Ϣ

        //��ILRuntime�ṩ��AppDomain��
        //��ILRuntime�ṩ�����ڽ���ִ��dll��pdb�ļ���
        //ͨ�������ǲ��ܽ���ִ�����ǵ��ȸ�����

        //�������þ��е�����xLua�е�LuaEnv lua������
        #endregion

        #region ֪ʶ��� Unity������ILRuntime
        //������ʽ
        //1.����AppDomain����(�����ռ� ILRuntime.Runtime.Enviorment)
        appDomain = new AppDomain();
        //2.���ر��ػ�Զ�����ص�dll��pdb�ļ�
        StartCoroutine(LoadHotUpdateInfo());
        //3.�����ص�������������ʽ(�ļ��������ڴ�������)���ݸ�AppDomain�����е�LoadAssembly����

        //4.��ʼ��ILRuntime�����Ϣ��Ŀǰֻ��Ҫ����ILRuntime���̵߳��߳�ID����ҪĿ�����ܹ���Unity��Profiler�����������з������⣩

        //5.ִ���ȸ������е��߼�

        //ע�⣺
        //һ����һ����Ŀ�У�����������ֻ��Ҫһ��AppDomain����
        #endregion

        #region �ܽ�
        //�ӱ���Unity������ILRuntime���Ǹ��ܹ����ܵ�
        //ILRuntime�ȸ��µ�������ʵ�����ȸ������е�dll�ļ���pdb�ļ�
        //�������ļ��оͰ������������е��ȸ�C#������Ϣ
        //�Ժ��������Ҫ��Զ���ȸ��£�ֻ��Ҫ�����Ƿ���AB�����ؼ���
        #endregion
    }

    /// <summary>
    /// ȥ�첽�������ǵ��ȸ���ص�dll��pdb�ļ�
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadHotUpdateInfo()
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
        //������������ر� ����ȵ�����Ҫ�����ȸ��߼����ܹر�
        //�����������ļ����ڴ������ڳ�ʼ�� appDomain ����֮��Ϳ���ͨ���ö�����ִ�����Ƕ�Ӧ���ȸ�������
        appDomain.LoadAssembly(dllStream, pdbStream, new PdbReaderProvider());

        //4.��ʼ��ILRuntime�����Ϣ��Ŀǰֻ��Ҫ����ILRuntime���̵߳��߳�ID����ҪĿ�����ܹ���Unity��Profiler�����������з������⣩
        appDomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;


        //5.ִ���ȸ������е��߼�
        print("�����Ѿ���ILRuntime��ʼ���ɹ� ֮��Ϳ���ִ�ж�Ӧ�߼���");
    }

    private void OnDestroy()
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
    }
}

using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson7 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ��ϰ ����ILRuntime ���������
        ILRuntimeMgr.Instance.StartILRuntime(()=> {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            //�õ�IType ͨ����ʵ����һ��ָ�����ȸ��¹����е������ (Lesson3_Test)
            IType type = appDomain.LoadedTypes["HotFix_Project.TestL3"];
            object obj = ((ILType)type).Instantiate();
            //1.����������ͬ
            //1-1.ͨ��appDomain.Invoke���ò���������ͬ��ʽ��������������Զ��ֱ�
            appDomain.Invoke("HotFix_Project.TestL3", "TestFunc", obj);
            appDomain.Invoke("HotFix_Project.TestL3", "TestFunc", obj, 1);
            //���ڲ���������ͬ ���� ���Ͳ�ͬʱ ʹ�����ַ�ʽ ���ܻᱨ�� ��Ϊ���ܹ���ȷ���õ���˭
            //appDomain.Invoke("HotFix_Project.Lesson3_Test", "TestFun", obj, 1.1f);

            //1-2.ͨ��GetMethod�ĵڶ�����������ȡ��Ӧ���������ĺ���
            IMethod method1 = type.GetMethod("TestFunc", 0);
            IMethod method2 = type.GetMethod("TestFunc", 1);

            appDomain.Invoke(method1, obj);
            appDomain.Invoke(method2, obj, 1);

            using(var method = appDomain.BeginInvoke(method1))
            {
                method.PushObject(obj);
                method.Invoke();
            }

            //2.����������ͬ�����Ͳ�ͬ��ͨ���������ַ�ʽֱ��ʹ���޷�ȷ��ȡ�����ĺ�����˭
            //  ������Ҫͨ��GetMethod��������ȡָ���������͵ĺ���
            //  2.1-��ȡ������Ӧ��IType���ͣ�����appDomain�е�GetType���� ��ȡָ���������͵�IType
            IType stringType = appDomain.GetType(typeof(string));
            //  2.2-��������б��У�����ȡ����IType����List<IType>��
            List<IType> list = new List<IType>();
            list.Add(stringType);
            //  2.3-����GetMethod�л�ȡָ�����Ͳ�����ʹ��GetMethod����һ�����أ�����ָ�����ͻ�ȡ������Ϣ
            method2 = type.GetMethod("TestFunc", list,new IType[]{appDomain.GetType(typeof(string))} ,appDomain.GetType(typeof(float)));
            using (var method = appDomain.BeginInvoke(method2))
            {
                string str = "lllllll";
                method.PushObject(obj);
                method.PushValueType(ref str);
                method.Invoke();
                var result = method.ReadFloat();
                Debug.Log(result);
            }

        });
        #endregion

        #region ֪ʶ�� ���ط�������
        //1.����������ͬ
        //1-1.ͨ��appDomain.Invoke���ò���������ͬ��ʽ��������������Զ��ֱ�

        //1-2.ͨ��GetMethod�ĵڶ�����������ȡ��Ӧ���������ĺ���

        //2.����������ͬ�����Ͳ�ͬ��ͨ���������ַ�ʽֱ��ʹ���޷�ȷ��ȡ�����ĺ�����˭
        //  ������Ҫͨ��GetMethod��������ȡָ���������͵ĺ���
        //  2.1-��ȡ������Ӧ��IType���ͣ�����appDomain�е�GetType���� ��ȡָ���������͵�IType
        //  2.2-��������б��У�����ȡ����IType����List<IType>��
        //  2.3-����GetMethod�л�ȡָ�����Ͳ�����ʹ��GetMethod����һ�����أ�����ָ�����ͻ�ȡ������Ϣ
        #endregion

        #region �ܽ�
        //1.�������ã�������ѭ���師���ù���
        //2.����������ͬʱ��ͨ����ȷ������������ȷ����
        //  ����������ͬ�����Ͳ�ͬʱ��ͨ��ָ��������������ȷ����
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson8 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ��ϰ ����ILRuntime ���������
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            //�õ�IType ͨ����ʵ����һ��ָ�����ȸ��¹����е������ (Lesson3_Test)
            IType type = appDomain.LoadedTypes["HotFix_Project.TestL3"];
            object obj = ((ILType)type).Instantiate();
            //���������ſ���
            //��Ҫͨ��IMethod��������
            //������Ҫʹ����GC Alloc��������

            IMethod methodName = type.GetMethod("TestFunc3", 3);
            List<int> list = new List<int>() { 1, 2, 3, 4 };
            using (var method = appDomain.BeginInvoke(methodName))
            {
                //1.������������һ���ĵط�
                //��Ҫ��ѹ��ref��out�����ĳ�ʼֵ
                //ѹ���һ��ref�����ĳ�ʼֵ
                method.PushObject(list);
                //ѹ��ڶ���out�����ĳ�ʼֵ ����out��������Ҫ���ⲿ��ʼ�� ����ѹ��null ����
                method.PushObject(null);

                //2.��������������д��һ��
                //ѹ����ö���
                method.PushObject(obj);
                //ѹ�������
                method.PushInteger(100);

                //3.ref��out��Ϊ��һ��ʼ��ѹ����ֵ
                //��������Ҫѹ�����ǵ�����λ��
                //ref��out���� ѹ�������������ֵ���� ��0��ʼ
                method.PushReference(0);
                method.PushReference(1);

                method.Invoke();

                //4.ͨ��Read��˳���ȡref/out������ֵ �ͷ���ֵ������ֵ����ȡ
                //��ȡref����
                list = method.ReadObject<List<int>>(0);
                //��ȡout����
                float f = method.ReadFloat(1);
                //��ȡ��������ֵ
                float returnValue = method.ReadInteger();

                print("ref���� list�ĳ��� " + list.Count);
                print("out������" + f);
                print("��������ֵ��" + returnValue);
            }
        });
        #endregion

        #region ֪ʶ�� ref/out��������
        //��Ҫͨ��IMethod��������
        //������Ҫʹ����GC Alloc��������

        //1.������������һ���ĵط�
        //��Ҫ��ѹ��ref��out�����ĳ�ʼֵ

        //2.��������������д��һ��
        //ѹ����ö���
        //ѹ�������

        //3.ref��out��Ϊ��һ��ʼ��ѹ����ֵ
        //��������Ҫѹ�����ǵ�����λ��
        //ref��out���� ѹ�������������ֵ���� ��0��ʼ

        //4.ͨ��Read��˳���ȡref/out������ֵ �ͷ���ֵ������ֵ����ȡ
        #endregion

        #region �ܽ�
        //1.ref/out����ֻ��ͨ����GC Alloc�������� using BeginInvoke push Invoke read -> ubpir��ʽ
        //2.�ڵ���ʱ������������
        //  2.1-��Ҫ��ѹ��ref��out�����ĳ�ʼֵ
        //  2.2-ѹ���������ѹ����������ֵ
        //  2.3-ͨ��Read��˳���ȡref��out����������ֵ����ȡ
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region ֪ʶ��һ ILRuntime�����
        //1.�޸Ĺ����ļ�����Packages�ļ����е�manifest.json�ļ�
        //  ��dependencies�ֶ�ǰ���ϣ�
        //"scopedRegistries": [
        //  {
        //    "name": "ILRuntime",
        //    "url": "https://registry.npmjs.org",
        //    "scopes": [
        //          "com.ourpalm"
        //    ]
        //  }
        //  ],
        //  ������PrejectSetting�е�Package Manager��Ӷ�Ӧ��Ϣ
        //  Name: ILRuntime
        //  Url: https://registry.npmjs.org
        //  Scopes: com.ourpalm

        //2.��Unity�������е�Window�д�Package Manager
        //  ���Ͻǵ�Packagesѡ��My Registriesѡ��
        //  ѡ�����е�ILRuntime����װ
        //  ��װ��ɺ� �ٵ�����ʾ������

        //3.��ʱ����ֱ�����ΪILRuntime�л��õ�C#�е�unsafe�ؼ���
        //  ������Ҫ�ڹ�������Edit��ѡ��ProjectSettings
        //  ��Other Setting �� ��ѡ Allow 'unsafe' Code ��ѡ��

        //4.ִ�������������󣬿����ڹ���������ILRuntime��ѡ��
        #endregion

        #region ֪ʶ��� ILRuntime�Ŀ�����ʽ
        //ͨ��֪ʶ��һ����ILRuntime�Ĺ��ߺ�
        //���ǿ����� ����Ŀ¼�µ�Samples\ILRuntime\2.1.0\Demo�ļ�����
        //����һ��HotFix_Project���ļ��У����ļ����ڱ༭�����޷�����
        //��Ҫ���ļ�������в鿴
        //�ù��̾������ǵ��ȸ�����

        //ILRuntime�Ŀ�����ʽ�ֳ�����������
        //1.ֱ����Unity�����н��з��ȸ����ֵĿ���������ǰдC#һ����
        //2.���ȸ������п�����Ҫ�ȸ��µĲ��֣�ͬ��ʹ��C#��
        //����ILRuntime����������Ҫ����������֮���л�����
        //��������֮������໥���ʵ���
        //������ڿ���ILRuntime�ȸ���Ŀʱ�Ĺ�����ʽ
        #endregion

        #region ֪ʶ���� ʲô��ILRuntime�еĿ������
        //ILRuntime�еĿ�����ʾ���ָ
        //��ԭʼ��Unity����֮�� �� �ȸ����� ֮����໥���ʵ���
        //���磺
        //��Unity������ʹ���ȸ����������������ݣ��ࡢί�С������ȣ�
        //���ȸ�������ʹ��Unity���������������ݣ��ࡢί�С������ȣ�
        #endregion

        #region ֪ʶ���� ILRuntime�Ļ���ԭ��
        //�ؼ���
        //Mono.Cecil��
        //����һ��ר�����ڶ�ȡC#�����DLL�Ŀ�Դ��������
        //ͨ�������ǿ��Ի�ȡ��
        //1.DLL�е����ͺͷ���ԭ��Ϣ
        //2.��ȡ�������IL���ָ��
        //3.���Զ�ȡPDB���Է��ű��ļ�
        //4.�����޸�DLL�е�Ԫ��Ϣ�ͷ��������ݲ�д��DLL

        //ILRuntimeͨ���ÿ�
        //������ʱͨ����ȡ����DLL�ļ��е�������ִ�������ȸ��Ĵ���
        #endregion

        #region ֪ʶ���� ִ�����ǵĵ�һ��ILRuntime�ȸ�����
        //1.ͨ���ļ��������HotFix_Project���̣�����������޸Ĺ��̵�Ŀ���ܣ�
        //2.���ɳɹ��󣬿�����Unity���̵�StreamingAssets�ļ����п���HotFix_Project�������ļ�
        //  һ����dll�ļ���һ����pdb�ļ�
        //3.��ʾ������HelloWorld�����С��ɹ��������Console���ڿ�����ӡ��Ϣ
        #endregion

        #region �ܽ�
        //��Ҫ����ʹ��ILRuntime��ע�����
        //1.�޸�manifest.json�ļ��е�����
        //2.Allow 'unsafe' Code ѡ��
        //3.ͨ��HotFix_Project�������������ȸ���dll��pdb�ļ�

        //ILRuntime�Ļ���ԭ���������Mono.Cecil��
        //ȥ����ִ���ȸ�������DLL�еĴ���
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

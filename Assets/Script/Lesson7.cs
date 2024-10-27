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
        #region 复习 启动ILRuntime 创建类对象
        ILRuntimeMgr.Instance.StartILRuntime(()=> {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            //得到IType 通过他实例化一个指定的热更新工程中的类对象 (Lesson3_Test)
            IType type = appDomain.LoadedTypes["HotFix_Project.TestL3"];
            object obj = ((ILType)type).Instantiate();
            //1.参数数量不同
            //1-1.通过appDomain.Invoke调用参数数量不同格式，传入参数即可自动分别
            appDomain.Invoke("HotFix_Project.TestL3", "TestFunc", obj);
            appDomain.Invoke("HotFix_Project.TestL3", "TestFunc", obj, 1);
            //对于参数数量相同 但是 类型不同时 使用这种方式 可能会报错 因为不能够明确调用的是谁
            //appDomain.Invoke("HotFix_Project.Lesson3_Test", "TestFun", obj, 1.1f);

            //1-2.通过GetMethod的第二个参数来获取对应参数个数的函数
            IMethod method1 = type.GetMethod("TestFunc", 0);
            IMethod method2 = type.GetMethod("TestFunc", 1);

            appDomain.Invoke(method1, obj);
            appDomain.Invoke(method2, obj, 1);

            using(var method = appDomain.BeginInvoke(method1))
            {
                method.PushObject(obj);
                method.Invoke();
            }

            //2.参数数量相同，类型不同，通过上面两种方式直接使用无法确定取出来的函数是谁
            //  我们需要通过GetMethod方法来获取指定参数类型的函数
            //  2.1-获取参数对应的IType类型，利用appDomain中的GetType方法 获取指定变量类型的IType
            IType stringType = appDomain.GetType(typeof(string));
            //  2.2-放入参数列表中，将获取到的IType放入List<IType>中
            List<IType> list = new List<IType>();
            list.Add(stringType);
            //  2.3-传入GetMethod中获取指定类型参数，使用GetMethod的另一个重载，传入指定类型获取方法信息
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

        #region 知识点 重载方法调用
        //1.参数数量不同
        //1-1.通过appDomain.Invoke调用参数数量不同格式，传入参数即可自动分别

        //1-2.通过GetMethod的第二个参数来获取对应参数个数的函数

        //2.参数数量相同，类型不同，通过上面两种方式直接使用无法确定取出来的函数是谁
        //  我们需要通过GetMethod方法来获取指定参数类型的函数
        //  2.1-获取参数对应的IType类型，利用appDomain中的GetType方法 获取指定变量类型的IType
        //  2.2-放入参数列表中，将获取到的IType放入List<IType>中
        //  2.3-传入GetMethod中获取指定类型参数，使用GetMethod的另一个重载，传入指定类型获取方法信息
        #endregion

        #region 总结
        //1.方法调用，还是遵循三板斧调用规则
        //2.参数数量不同时，通过明确参数数量来明确重载
        //  参数数量相同，类型不同时，通过指明参数类型来明确重载
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

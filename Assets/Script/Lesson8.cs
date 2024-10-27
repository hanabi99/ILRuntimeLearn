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
        #region 复习 启动ILRuntime 创建类对象
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            AppDomain appDomain = ILRuntimeMgr.Instance.appDomain;
            //得到IType 通过他实例化一个指定的热更新工程中的类对象 (Lesson3_Test)
            IType type = appDomain.LoadedTypes["HotFix_Project.TestL3"];
            object obj = ((ILType)type).Instantiate();
            //必须这样才可以
            //需要通过IMethod方法调用
            //并且需要使用无GC Alloc方法调用

            IMethod methodName = type.GetMethod("TestFunc3", 3);
            List<int> list = new List<int>() { 1, 2, 3, 4 };
            using (var method = appDomain.BeginInvoke(methodName))
            {
                //1.和其他函数不一样的地方
                //需要先压入ref或out参数的初始值
                //压入第一个ref参数的初始值
                method.PushObject(list);
                //压入第二个out参数的初始值 由于out参数不需要在外部初始化 所以压入null 即可
                method.PushObject(null);

                //2.和其它函数调用写法一致
                //压入调用对象
                method.PushObject(obj);
                //压入各参数
                method.PushInteger(100);

                //3.ref和out因为在一开始就压入了值
                //在这里需要压入他们的索引位置
                //ref和out参数 压入参数引用索引值即可 从0开始
                method.PushReference(0);
                method.PushReference(1);

                method.Invoke();

                //4.通过Read按顺序获取ref/out参数的值 和返回值，返回值最后获取
                //获取ref参数
                list = method.ReadObject<List<int>>(0);
                //获取out参数
                float f = method.ReadFloat(1);
                //获取函数返回值
                float returnValue = method.ReadInteger();

                print("ref参数 list的长度 " + list.Count);
                print("out参数：" + f);
                print("函数返回值：" + returnValue);
            }
        });
        #endregion

        #region 知识点 ref/out方法调用
        //需要通过IMethod方法调用
        //并且需要使用无GC Alloc方法调用

        //1.和其他函数不一样的地方
        //需要先压入ref或out参数的初始值

        //2.和其它函数调用写法一致
        //压入调用对象
        //压入各参数

        //3.ref和out因为在一开始就压入了值
        //在这里需要压入他们的索引位置
        //ref和out参数 压入参数引用索引值即可 从0开始

        //4.通过Read按顺序获取ref/out参数的值 和返回值，返回值最后获取
        #endregion

        #region 总结
        //1.ref/out方法只能通过无GC Alloc方法调用 using BeginInvoke push Invoke read -> ubpir方式
        //2.在调用时多了三个步骤
        //  2.1-需要先压入ref或out参数的初始值
        //  2.2-压入参数环节压入引用索引值
        //  2.3-通过Read按顺序获取ref、out参数，返回值最后获取
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

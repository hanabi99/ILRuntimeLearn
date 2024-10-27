using System;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using UnityEngine;


public class L3 : MonoBehaviour
{
    private void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            var appDomain = ILRuntimeMgr.Instance.appDomain;
            //方式一
            object l3 =  appDomain.Instantiate("HotFix_Project.TestL3",new object []{ "123"});
            string str = appDomain.Invoke("HotFix_Project.TestL3", "get_Str", l3).ToString();
            Debug.Log(str);
            //方式二 推荐
            var type = appDomain.LoadedTypes["HotFix_Project.TestL3"];
            object l3_1 = ((ILType)type).Instantiate(new object[] { "2222" });
            Debug.Log(l3_1);
            //调用属性
            IMethod getStr = type.GetMethod("get_Str", 0);
            IMethod setStr = type.GetMethod("set_Str", 1); 
            str = appDomain.Invoke(getStr, l3_1).ToString();
            Debug.Log(str);
            appDomain.Invoke(setStr, l3_1, "3333");
            str = appDomain.Invoke(getStr, l3_1).ToString();
            Debug.Log(str);
            //成员方法
            IMethod span = type.GetMethod("Span", 1);
            appDomain.Invoke(span, l3_1, "4444");
            str = appDomain.Invoke(getStr, l3_1).ToString();
            Debug.Log(str);
            //无GC Alloc
            using (var method = appDomain.BeginInvoke(setStr))
            {
                method.PushObject(l3_1);
                var str1 = "8888";
                method.PushValueType<string>(ref str1);
                method.Invoke();
            }
            using (var method = appDomain.BeginInvoke(getStr))
            {
                method.PushObject(l3_1);
                method.Invoke();
                str = method.ReadValueType<string>();
                Debug.Log(str);
            }
            //方式三
            // var constructorInfo = type.ReflectionType.GetConstructor(new Type[]{typeof(string)});
            // if (constructorInfo != null)
            // {
            //     var info =constructorInfo.Invoke(new object[] { "12222" });
            //     Debug.Log(info);
            // }
            
        });
    }
}

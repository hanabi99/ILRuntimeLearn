using System.Collections;
using System.Collections.Generic;
using ILRuntime.CLR.Method;
using UnityEngine;

public class L5 : MonoBehaviour
{

    void Start()
    {
        ILRuntimeMgr.Instance.StartILRuntime(() =>
        {
            var appDomain = ILRuntimeMgr.Instance.appDomain;
            //调用静态方法
           var num = appDomain.Invoke("HotFix_Project.TestL3", "StaticTest", null, "999");
           Debug.Log(num);

           IMethod method = appDomain.LoadedTypes["HotFix_Project.TestL3"].GetMethod("StaticTest", 1);
           num = appDomain.Invoke(method, null, "8888");
           Debug.Log(num);

           using (var context = appDomain.BeginInvoke(method))
           { 
               var str = "222";
               context.PushValueType<string>(ref str);
               context.Invoke();
               num = context.ReadInteger();
               Debug.Log(num);
           }
        });
    }
}

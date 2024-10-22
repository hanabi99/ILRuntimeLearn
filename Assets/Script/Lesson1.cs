using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 ILRuntime环境搭建
        //1.修改工程文件夹下Packages文件夹中的manifest.json文件
        //  在dependencies字段前加上：
        //"scopedRegistries": [
        //  {
        //    "name": "ILRuntime",
        //    "url": "https://registry.npmjs.org",
        //    "scopes": [
        //          "com.ourpalm"
        //    ]
        //  }
        //  ],
        //  或者在PrejectSetting中的Package Manager添加对应信息
        //  Name: ILRuntime
        //  Url: https://registry.npmjs.org
        //  Scopes: com.ourpalm

        //2.在Unity工具栏中的Window中打开Package Manager
        //  左上角的Packages选择My Registries选项
        //  选择其中的ILRuntime并安装
        //  安装完成后 再导入其示例工程

        //3.这时会出现报错，因为ILRuntime中会用到C#中的unsafe关键字
        //  我们需要在工具栏的Edit中选择ProjectSettings
        //  在Other Setting 中 勾选 Allow 'unsafe' Code 的选项

        //4.执行完上述操作后，可以在工具栏看到ILRuntime的选项
        #endregion

        #region 知识点二 ILRuntime的开发方式
        //通过知识点一导入ILRuntime的工具后
        //我们可以在 工程目录下的Samples\ILRuntime\2.1.0\Demo文件夹中
        //看到一个HotFix_Project的文件夹，该文件夹在编辑器中无法看到
        //需要在文件浏览器中查看
        //该工程就是我们的热更工程

        //ILRuntime的开发方式分成了两个部分
        //1.直接在Unity工程中进行非热更部分的开发（和以前写C#一样）
        //2.在热更工程中开发需要热更新的部分（同样使用C#）
        //导入ILRuntime过后我们需要在两个工程之间切换开发
        //两个工程之间可以相互访问调用
        //这就是在开发ILRuntime热更项目时的工作方式
        #endregion

        #region 知识点三 什么是ILRuntime中的跨域访问
        //ILRuntime中的跨域访问就是指
        //在原始的Unity工程之间 和 热更工程 之间的相互访问调用
        //比如：
        //在Unity工程中使用热更工程中声明的内容（类、委托、函数等）
        //在热更工程中使用Unity工程中声明的内容（类、委托、函数等）
        #endregion

        #region 知识点四 ILRuntime的基本原理
        //关键点
        //Mono.Cecil库
        //它是一个专门用于读取C#编译的DLL的开源第三方库
        //通过它我们可以获取到
        //1.DLL中的类型和方法原信息
        //2.读取方法体的IL汇编指令
        //3.可以读取PDB调试符号表文件
        //4.可以修改DLL中的元信息和方法体内容并写回DLL

        //ILRuntime通过该库
        //在运行时通过读取解译DLL文件中的内容来执行我们热更的代码
        #endregion

        #region 知识点五 执行我们的第一个ILRuntime热更程序
        //1.通过文件浏览器打开HotFix_Project工程（如果报错，请修改工程的目标框架）
        //2.生成成功后，可以在Unity工程的StreamingAssets文件夹中看到HotFix_Project的两个文件
        //  一个是dll文件、一个是pdb文件
        //3.打开示例工程HelloWorld，运行。成功后可以在Console窗口看到打印信息
        #endregion

        #region 总结
        //想要正常使用ILRuntime的注意事项：
        //1.修改manifest.json文件中的配置
        //2.Allow 'unsafe' Code 选项
        //3.通过HotFix_Project工程生成用于热更的dll和pdb文件

        //ILRuntime的基本原理就是利用Mono.Cecil库
        //去解释执行热更工程中DLL中的代码
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

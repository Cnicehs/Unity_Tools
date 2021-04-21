using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEditorInternal;

public class Replacer : EditorWindow
{
    [MenuItem("Tools/Relacer")]//在unity菜单Window下有MyWindow选项
    static void Init()
    {
        Replacer myWindow = (Replacer)EditorWindow.GetWindow(typeof(Replacer), false, "Replacer", true);//创建窗口
        myWindow.Show();//展示
    }

    Component temple;
    Transform root;
    bool ifRegex;
    // bool ifUsePosition = true;
    bool asChild;
    bool showChecked = true;
    public string pattren="";
    void OnGUI()
    {
        var properties = typeof(UISprite).GetProperties();
        temple = EditorGUILayout.ObjectField("Temple", temple, typeof(Component),true) as Component;
        root = EditorGUILayout.ObjectField("Root", root, typeof(Transform), true) as Transform;
        if(temple==null||root==null) 
            return;
        asChild = EditorGUILayout.Toggle("将temple作为子物体", asChild);
        if(!asChild)
            ifRegex = EditorGUILayout.Toggle("Use regex?，default Temple name", ifRegex);
        else
            ifRegex = true;
        if(ifRegex)
            pattren = EditorGUILayout.TextField("正则", pattren);
        else
            pattren = temple.name;

        // ifUsePosition = EditorGUILayout.Toggle("是否使用temple的Transform", ifUsePosition);
        
        if(GUILayout.Button("Start"))
        {
            if(asChild)
            {
                foreach (var item in root.GetComponentsInChildren(typeof(Transform), true))
                {
                    if (!Regex.IsMatch(item.name, pattren))
                        continue;
                    LogUtil.LogError(item.name);

                    GameObject.Instantiate(temple,item.transform);

                    // if (ComponentUtility.CopyComponent(temple))
                    // {
                    //     ComponentUtility.PasteComponentValues(item);
                    // }
                }
            }else
            {
                foreach (var item in root.GetComponentsInChildren(temple.GetType(), true))
                {
                    if (!Regex.IsMatch(item.name, pattren))
                        continue;
                    LogUtil.LogError(item.name);

                    // item.spriteName = temple.spriteName;
                    // item.atlas = temple.atlas;
                    // item.keepAspectRatio = temple.keepAspectRatio;
                    // item.aspectRatio = temple.aspectRatio;
                    // item.width = temple.width;
                    // item.height = temple.height;
                    // item.pivot = temple.pivot;


                    // if(item.GetType() == temple.GetType())
                    // {
                    if (ComponentUtility.CopyComponent(temple))
                    {
                        ComponentUtility.PasteComponentValues(item);
                    }
                    // }


                    // if(ifUsePosition)
                    // {
                    //     item.transform.localScale = temple.transform.localScale;
                    //     item.transform.localEulerAngles = temple.transform.localEulerAngles;
                    //     item.transform.localPosition = temple.transform.localPosition;
                    // }
                }
            }
        }

        showChecked = EditorGUILayout.Toggle("预览选中物体", showChecked);
        if(showChecked)
        {
            Object tmp;
            foreach (var item in root.GetComponentsInChildren(asChild?typeof(Transform):temple.GetType(), true))
            {
                if (!Regex.IsMatch(item.name, pattren))
                    continue;
                // if (item.GetType() != temple.GetType())
                //     continue;
                tmp = EditorGUILayout.ObjectField("Temple", item, typeof(UISprite), true) as UISprite;
            }
        }
    }
}

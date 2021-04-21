using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEditorInternal;
using System.Linq;

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
    public Vector2 scroll;
    void OnGUI()
    {
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
        List<Component> components = new List<Component>();

        //GetAllComponents
        foreach (var item in root.GetComponentsInChildren(asChild ? typeof(Transform) : temple.GetType(), true))
        {
            if (!Regex.IsMatch(item.name, pattren))
                continue;
            components.Add(item);
        }


        if(GUILayout.Button("Start"))
        {
                foreach (var item in components)
                {
                    Debug.LogError(item.name);
                    if(asChild)
                    {
                        GameObject.Instantiate(temple, item.transform);
                    }else
                    {
                        if (ComponentUtility.CopyComponent(temple))
                        {
                            ComponentUtility.PasteComponentValues(item);
                        }
                    }
                }
        }

        // showChecked = EditorGUILayout.Toggle("预览选中物体", showChecked);
        showChecked = EditorGUILayout.Foldout(showChecked, "选中的对象");
        if(showChecked)
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            Object tmp;
            foreach (var item in root.GetComponentsInChildren(asChild?typeof(Transform):temple.GetType(), true))
            {
                if (!Regex.IsMatch(item.name, pattren))
                    continue;
                // if (item.GetType() != temple.GetType())
                //     continue;
                tmp = EditorGUILayout.ObjectField("Temple", item, item.GetType(),true);
            }
            EditorGUILayout.EndScrollView();
        }
    }

    bool AncestorIs(Transform obj,params string[] names)
    {
        while(obj.parent!=null)
        {
            if(names.Contains(obj.parent.name))
                return true;
            obj = obj.parent;
        }
        return false;
    }
}

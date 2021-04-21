using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEditorInternal;
using System.Linq;
using System;

public class Replacer : EditorWindow
{
    [MenuItem("Tools/Relacer")]//在unity菜单Window下有MyWindow选项
    static void Init()
    {
        Replacer myWindow = (Replacer)EditorWindow.GetWindow(typeof(Replacer), false, "Replacer", true);//创建窗口
        myWindow.Show();//展示
    }
    List<Component> temples = new List<Component>();
    // Component temple;
    Transform root;
    bool ifRegex;
    // bool ifUsePosition = true;
    bool asChild;
    bool showChecked = true;
    public string pattren = "";
    public string excludeParentRegex = "";
    public Vector2 scroll;
    public int nums = 1;
    void OnGUI()
    {
        nums = EditorGUILayout.IntField("Componet数量", nums);
        for (int i = 0; i < nums - temples.Count; i++)
        {
            temples.Add(null);
        }
        for (int i = 0; i < temples.Count; i++)
        {
            if (i < nums)
            {
                temples[i] = EditorGUILayout.ObjectField("Temple" + (i + 1), temples[i], typeof(Component), true) as Component;
            }
            else
            {
                temples.RemoveAt(i--);
            }
        }
        // temple = EditorGUILayout.ObjectField("Temple", temple, typeof(Component),true) as Component;
        root = EditorGUILayout.ObjectField("Root", root, typeof(Transform), true) as Transform;
        // if(temple==null||root==null) 
        if (temples == null || root == null)
            return;
        asChild = EditorGUILayout.Toggle("将temple作为子物体", asChild);
        // if(!asChild)
        //     ifRegex = EditorGUILayout.Toggle("Use regex?，default Temple name", ifRegex);
        // else
        //     ifRegex = true;
        // if(ifRegex)
        pattren = EditorGUILayout.TextField("正则表达式-目标", pattren);
        // else
        //     pattren = temple.name;
        excludeParentRegex = EditorGUILayout.TextField("正则表达式-排除祖先节点", excludeParentRegex);

        // ifUsePosition = EditorGUILayout.Toggle("是否使用temple的Transform", ifUsePosition);
        Dictionary<Type,List<Component>>  components = new Dictionary<Type, List<Component>>();

        // foreach (var temple in temples)
        // {
        //GetAllComponents
        // foreach (var item in root.GetComponentsInChildren(asChild ? typeof(Transform) : temple.GetType(), true))
        foreach (var item in root.GetComponentsInChildren(typeof(Transform), true))
        {
            if (!Regex.IsMatch(item.name, pattren))
                continue;
            if (AncestorIs(item.transform, excludeParentRegex))
                continue;
            if (asChild)
            {
                if(!components.ContainsKey(typeof(Transform)))
                {
                    components.Add(typeof(Transform), new List<Component>());
                }
                components[typeof(Transform)].Add(item.transform);
            }
            else
            {
                foreach (var temple in temples)
                {
                    var comp = item.GetComponent(temple.GetType());
                    if (comp)
                    {
                        if (!components.ContainsKey(temple.GetType()))
                        {
                            components.Add(temple.GetType(), new List<Component>());
                        }
                        components[temple.GetType()].Add(comp);
                    }
                }
            }
        }
        // }

        if (GUILayout.Button("Start"))
        {
            foreach (var temple in temples)
            {
                foreach (var item in components[temple.GetType()])
                {
                    Debug.LogError(item.name);
                    if (asChild)
                    {
                        GameObject.Instantiate(temple, item.transform);
                    }
                    else
                    {
                        if (ComponentUtility.CopyComponent(temple))
                        {
                            ComponentUtility.PasteComponentValues(item);
                        }
                    }
                }
            }
        }

        // showChecked = EditorGUILayout.Toggle("预览选中物体", showChecked);
        showChecked = EditorGUILayout.Foldout(showChecked, "选中的对象");
        if (showChecked)
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            UnityEngine.Object tmp;
            int i = 0;
            foreach (var key in components.Keys)
            {
                foreach(var item in components[key])
                {
                    tmp = EditorGUILayout.ObjectField((++i).ToString(), item, item.GetType(), true);
                }
            }
            EditorGUILayout.EndScrollView();
        }

    }

    bool AncestorIs(Transform obj, string names)
    {
        while (obj.parent != null)
        {
            if (Regex.IsMatch(obj.parent.name, names))
                return true;
            obj = obj.parent;
        }
        return false;
    }
}

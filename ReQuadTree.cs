using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public interface IQuad
{
    Vector2 GetPosition();
    Rect GetRect();
}
public class DynamicQuadTree<T> : IQuad where T : IQuad
{
    public DynamicQuadTree<T>[] subtrees;
    public List<T> storedItems = new List<T>();
    public List<T> commonItems = new List<T>();
    private Rect rect;
    public Vector2 center
    {
        get
        {
            return rect.center;
        }
    }

    public float width
    {
        get
        {
            return rect.width;
        }
    }

    public float height
    {
        get
        {
            return rect.height;
        }
    }

    public DynamicQuadTree(Vector2 center, float width, float height)
    {
        rect = new Rect(center.x - width / 2, center.y - height / 2, width, height);
    }

    public Vector2 GetPosition()
    {
        return rect.center;
    }

    public Rect GetRect()
    {
        return rect;
    }

    public void Insert(T insertItem)
    {
        storedItems.Add(insertItem);
        if(subtrees==null)
        if(commonItems.Count+storedItems.Count<5)
        {
            return;
        }

        //
        if(subtrees==null)
        {
            //Init SubTree
            subtrees = new DynamicQuadTree<T>[4];
            subtrees[0] = new DynamicQuadTree<T>(center + new Vector2(width / 4, height / 4), width / 2, height / 2);
            subtrees[1] = new DynamicQuadTree<T>(center + new Vector2(-width / 4, height / 4), width / 2, height / 2);
            subtrees[2] = new DynamicQuadTree<T>(center + new Vector2(-width / 4, -height / 4), width / 2, height / 2);
            subtrees[3] = new DynamicQuadTree<T>(center + new Vector2(width / 4, -height / 4), width / 2, height / 2);
        }

        //Push storeItems to this Tree or subTree
        for (var i = storedItems.Count - 1; i > -1; i--)
        {
            foreach (var tree in subtrees)
            {
                if (tree.Contains(storedItems[i]))
                {
                    tree.Insert(storedItems[i]);
                    goto BreakInsert;
                }
            }
            commonItems.Add(storedItems[i]);
            BreakInsert:
                storedItems.RemoveAt(i);
        }
    }

    public bool Contains(T item)
    {
        Vector2 max = item.GetRect().max;
        Vector2 min = item.GetRect().min;

        return rect.Contains(max) && rect.Contains(min);
    }

    public void DrawGizom(int depth)
    {
        if(subtrees!=null)
        foreach (var tree in subtrees)
        {
            tree.DrawGizom(depth + 20);
        }
        float r = ((float)(depth % 255))/255f;
        float g = (Mathf.Pow(depth % 255, 2))%255/255f;
        float b = (Mathf.Pow(depth % 255, 3)) % 255 / 255f;
        Gizmos.color = new Color(r+0.3f, g+0.2f, b+0.1f);
        Gizmos.DrawWireCube(rect.center, new Vector3(rect.width, rect.height, 0));
    }
}

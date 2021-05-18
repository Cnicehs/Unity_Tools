using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReQuadTreeTest : MonoBehaviour
{
    public List<QuadItem> items;
    public DynamicQuadTree<QuadItem> tree;

    private void Start()
    {
        tree = new DynamicQuadTree<QuadItem>(Vector2.zero, 100, 100);
        foreach (var item in items)
        {
            tree.Insert(item);
        }
    }

    private void OnDrawGizmos()
    {
        if(tree!=null)
        tree.DrawGizom(0);
    }
}

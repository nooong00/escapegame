using UnityEngine;

[System.Serializable]
public class ItemData  {
    public int key;
    public bool having;
    public int path;
    public int pay;
    public ItemType type;


    public ItemData(int k, int p, int pa, ItemType t)
    {
        key = k;
        path = p;
        having = false;
        pay = pa;
        type = t;
    }

    public void SetData(ItemData d)
    {
        key = d.key;
        having = d.having;
        path = d.path;
        pay = d.pay;
        type = d.type;
    }
}

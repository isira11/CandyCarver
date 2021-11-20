using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EasyButtons;

[CreateAssetMenu(menuName = "CandySo", order = 1)]
public class ShapesSO : ScriptableObject
{
    public List<Texture2D> textures;

    [Button]
    public void CreateData()
    {
        candyDatas.Clear();

        Hashtable t = new Hashtable();
        List<string> ids = new List<string>();

        foreach (Texture2D item in textures)
        {
            string id = item.name.Split('_')[0];
            string t2 = item.name.Split('_')[1];

            if (!t.ContainsKey(id))
            {
                t[id] = new List<Texture2D>();
                ids.Add(id);
            }

            ((List<Texture2D>)t[id]).Add(item);
        }

        foreach (string id in ids)
        {
            try
            {
                List<Texture2D> item = (List<Texture2D>)t[id];
                CandyData cd = new CandyData();
                candyDatas.Add(cd);
                cd.id = id;
                cd.t1 = item[0];
                cd.t2 = item[1];
                cd.t3 = item[2];
                cd.t4 = item[3];
                cd.t5 = item[4];
            }
            catch (Exception ex)
            {

            }
        }

        textures.Clear();
    }

    public List<CandyData> candyDatas;
}

[Serializable]
public class CandyData
{
    public string id;
    public Texture2D t1;
    public Texture2D t2;
    public Texture2D t3;
    public Texture2D t4;
    public Texture2D t5;
    public bool ad_unloackable;
}

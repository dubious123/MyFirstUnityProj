using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPenel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach(Transform child in gridPenel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        for(int i = 0; i < 8; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridPenel.transform).gameObject;
            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
            invenItem.SetInfo($"�����{i}��");

        }
    }
    void Update()
    {
        
    }
}

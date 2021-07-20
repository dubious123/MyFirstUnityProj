using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{

    enum Buttons
    {
        PointButton
    }
    enum Texts
    {
        PointText,
        ScoreText
    }
    enum GameObjects
    {
        TestObjects,
    }
    enum Images
    {
        ItemIcon,
    }

    int _score = 0;
    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        Get<Text>((int)Texts.ScoreText).text = $"���� : {_score}";
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
  


        GetButton((int)Buttons.PointButton).gameObject.AddUIEvent(OnButtonClicked);
        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        AddUIEvent(go,(PointerEventData data)=> { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }
}
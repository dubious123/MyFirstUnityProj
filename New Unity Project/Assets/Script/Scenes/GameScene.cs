using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameScene : BaseScene
{
    //class Test
    //{
    //    public int id = 0;
    //}
    //class CoroutineTest : IEnumerable
    //{
    //    public IEnumerator GetEnumerator()
    //    {
    //        yield return new Test() { id = 1 };
    //        yield return new Test() { id = 2 };
    //        yield return new Test() { id = 3 };
    //        //�������� �����ϰ� ���� ��
         
    //        yield return new Test() { id = 4 };
    //        yield return new Test() { id = 5 };
    //        yield break;
    //    }
    //}
    //coroutine
    //void VeryComplicated()
    //{
    //    for(int i = 0; i < 1000000; i++)
    //    {
    //        if(i%10000 == 0)
    //        {
    //            yield return null;
    //        }
    //        //���� �۾�
    //        Debug.Log("Hello");
    //    }
    //}
    //�Ϸ��� ������ �˰����� ��ƽ�� �ϰ� ����� �� ������
    //������ �ʿ��ϴ�.
    //�Ͻ���������� ������ ������ ����
    //c#�� ������ C++���� ���� ����
    //��û �����ɸ��� �۾��� ��� ���ų�
    //���ϴ� Ÿ�ֿ̹� �Լ��� ��� stop, �����ϴ� ���
    //return -> �츮�� ���ϴ� Ÿ������ ����


    //4�ʰ� ���� ���� ��ų ����
    //������ ��ƽ���� �ð��� üũ�ؾ��Ѵٴ� ���� �����̴�.
    //������ �ð��Ŵ����� �־ �߾ӿ��� �ѹ��� �����Ѵ�.
    //�̶� �ڷ�ƾ�� �����ϴ�.
    void GenerateItem()
    {
        //�������� ������ش�
        //������ ����
        //DB����
        //DB���� ����
        //����
        //�ۺ��� !!


        //������ ��� ����ٰ� DB���� ������ �޾ƿ��� �ٽ� ������ ����
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;


        Managers.UI.ShowSceneUI<UI_Inven>();

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        gameObject.GetOrAddComponent<CursorController>();
    }

    public override void Clear()
    {

    }

}

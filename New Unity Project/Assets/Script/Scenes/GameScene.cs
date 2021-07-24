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
    //        //정말정말 종료하고 싶을 때
         
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
    //        //어마어마한 작업
    //        Debug.Log("Hello");
    //    }
    //}
    //일렇게 복잡한 알고리즘을 한틱에 하게 만들면 좀 오바임
    //분할이 필요하다.
    //일시정지기능이 있으면 좋을것 같아
    //c#은 있으나 C++에는 아직 없어
    //엄청 오래걸리는 작업을 잠시 끊거나
    //원하는 타이밍에 함수를 잠시 stop, 복원하는 경우
    //return -> 우리가 원하는 타입으로 가능


    //4초가 지난 이후 스킬 실행
    //원래는 매틱마다 시간을 체크해야한다는 것이 단점이다.
    //보통은 시간매니저가 있어서 중앙에서 한번만 관리한다.
    //이때 코루틴이 유용하다.
    void GenerateItem()
    {
        //아이템을 만들어준다
        //아이템 생성
        //DB저장
        //DB저장 실패
        //로직
        //템복사 !!


        //로직을 잠시 멈췄다가 DB에서 성공을 받아오면 다시 로직을 실행
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

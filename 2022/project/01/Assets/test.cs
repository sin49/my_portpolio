using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class test : MonoBehaviour
{
    /* public bool test_bool;
     public float timer;
     // Start is called before the first frame update
     void Start()
     {
         StartCoroutine(test_corutine());
     }
     private void FixedUpdate()
     {
         if(test_bool)
         timer += Time.deltaTime;
     }
     IEnumerator test_corutine()
     {

             Debug.Log("A");
             Debug.Log("B");
         yield return new WaitUntil(() => timer>=5.0f);
             Debug.Log("END");


     }*/

    /** public Transform target_chr;
     float speed;
     Vector3 target_velocity;
     public float timer;
     public float time;
     Rigidbody rgd;
     void calculate_vector()
     {
         var vector = target_chr.position - transform.position;
         float magni = vector.magnitude;
         target_velocity = vector.normalized;
         target_velocity = target_velocity * (magni / (time-timer));
     }
     private void Start()
     {
         rgd = GetComponent<Rigidbody>();
         //calculate_vector();
     }
     private void FixedUpdate()
     {
         calculate_vector();
        
         if (timer >= time)
         {
             Debug.Log("111");
             rgd.velocity = Vector3.zero;
         }
         else
         {
             rgd.velocity = target_velocity;
             timer += Time.deltaTime;
         }
     }*/

    public int round_num;
    public List<chracter> bb=new List<chracter>();//종류
    public chracter_list[] aa;
    public List<List<chracter_info>> cc = new List<List<chracter_info>>();//실제 생성
    private void Start()
    {
        
    }
    public void Add_T_List_index(List<chracter_info> L, chracter c)
    {
        chracter_info c_info = new chracter_info(c);
        L.Add(c_info);
    }
    public void delete_T_List_index(List<chracter_info> L,int n)
    {
        L.RemoveAt(n);
    }
    public void dynamic_stage_list(int i)
    {
        
        if (cc.Count == i)
            return;
        int min = i-cc.Count;
        if (min > 0)
        {
            for(int n = 0; n < min; n++)
            {
                List<chracter_info> L = new List<chracter_info>();
                cc.Add(L);
            }
        }
        else
        {
            for(int n = cc.Count; n > i; n--)
            {
                cc.RemoveAt(n - 1);
            }
        }

    }
}

[System.Serializable]
public struct chracter_list
{
    public List<chracter_info> chr1;
}
[System.Serializable]
public struct chracter
{
    public GameCharacter chr;
    public int LV;
}

//struct내부 변수는 겟터함수로 참조되기에 직접 할당시 값이 복사가 되어서 특정  값을 지정해야하는 코드에는 에러코드 발생 참조:https://learn.microsoft.com/ko-kr/dotnet/csharp/language-reference/compiler-messages/cs1612

[System.Serializable]
public class chracter_info
{
    public chracter_info(chracter c)
    {
        chr = c;
        number = 1;
    }
    public chracter chr;
    public int number;
}
//결론 float값  하나 더 만들고 타이머 값 걸어서 LB후 다시 재생되는 움직임 구현 위치는 액션 실행중 return시키는 고세 else달아서
//원하는 시간에 특정 위치까지 이동하게 만드는 계산식target_velocity = target_velocity * (magni / time);(거리 바뀌면 재계산X->start에 사용);
//target_velocity = target_velocity * (magni / (time-timer));(거리에 따라 재계산O->update같은 frame 갱신에 응용)
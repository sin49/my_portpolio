using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenTitle : MonoBehaviour
{
    [Header("타이틀")]
    public Image AchImage;
    public Text AchName;

    [SerializeField]Inven_Book_Button Inven_p;

    [Header("인벤 버튼 선택")]
    public GameObject Total_ContentPlace;
    public List<Toggle> Total_Content;

    [Header("인벤요소")]
    public GameObject Inven_ContentPlace;
    public List<GameObject> Inven_Content;

    [Header("스페셜인벤요소")]
    public GameObject SpInven_ContentPlace;
    public List<GameObject> spInven_Content;

    [Header("시너지요소")]
    public GameObject synerg_ContentPlace;
    public List<GameObject> synerg_Content;
    public Text PageText;

    [Header("내용 출력")]
    public Image AchImage_C;
    public Text AchName_C;
    public Text AchConect;

    public Toggle My_toggle;

    [Header("인벤패널")]
    [SerializeField] List<int> PageNumberlimit = new List<int>();
    [SerializeField] List<int> PageNow = new List<int>();
    [SerializeField] List<int> PageTotal= new List<int>();

    public int ActiveButton;
    public Button R_Btn;
    public Button L_Btn;

    public int key;
    int num;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        My_toggle = this.gameObject.GetComponent<Toggle>();
        for(int i=0; i<Total_ContentPlace.transform.childCount;i++)
        {
            Total_Content.Add(Total_ContentPlace.transform.GetChild(i).GetComponent<Toggle>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(My_toggle.isOn)
        {
            ButtonOn();
        }
        else
        {

        }
    }

    public void ChangeElement(Inven_Book_Button Inven_b)        //버튼의 요소
    {
        this.Inven_p = Inven_b;
        AchImage.sprite = Inven_p.Inven_Button_Image;
        AchName.text = Inven_p.Inven_Button_Name;
        key = Inven_b.key-1;
    }
    public void ChangeElement_panel()        //패널의 요소
    {
        if (this.gameObject.activeSelf)
        {
            switch (key)
            {
                case 0:
                    synerg_Inven();
                    break;
                case 1:
                    inven_Inven();
                    break;
                case 2:
                    Sp_Inven();
                    break;
                default:
                    break;
            }
        }
    }

    public void synerg_Inven()
    {
        Debug.Log("시너지출력");
        ActiveButton = 0;
        for (int i = (PageNow[key] - 1) * PageNumberlimit[key]; i < PageNow[key] * PageNumberlimit[key]; i++)
        {
            if (i >= Synergy_Manager.Sy_manager.Sy_total_List.Count)
            {
                synerg_Content[i % PageNumberlimit[key]].gameObject.SetActive(false);
            }
            else
            {
                synerg_Content[i % PageNumberlimit[key]].gameObject.SetActive(true);
                ActiveButton++;
                //Debug.Log(ItemDatabase.itemDatabase.item_list[i].Name);
                synerg_Content[i % PageNumberlimit[key]].GetComponent<Inven_Content_Sy>().ChangeSy(Synergy_Manager.Sy_manager.Sy_total_List[i]);
            }
        }
    }

    public void inven_Inven()
    {
        Debug.Log("인벤 출력");
        ActiveButton = 0;
        for (int i = (PageNow[key] - 1) * PageNumberlimit[key]; i < PageNow[key] * PageNumberlimit[key]; i++)
        {
            if (i >= ItemDatabase.itemDatabase.GetItemList.Count)
            {
                Inven_Content[i % PageNumberlimit[key]].gameObject.SetActive(false);
            }
            else
            {
                Inven_Content[i % PageNumberlimit[key]].gameObject.SetActive(true);
                ActiveButton++;
                //Debug.Log(ItemDatabase.itemDatabase.item_list[i].Name);
                Inven_Content[i % PageNumberlimit[key]].GetComponent<Inven_Content>().ChangeAch(ItemDatabase.itemDatabase.GetItemList[i]);
            }
        }
    }
    public void Sp_Inven()
    {
        ActiveButton = 0;
        for (int i = (PageNow[key] - 1) * PageNumberlimit[key]; i < PageNow[key] * PageNumberlimit[key]; i++)
        {
            if (i >= Sp_ItemEffect.sp_itemeffect.Sp_have.Count)
            {
                spInven_Content[i % PageNumberlimit[key]].gameObject.SetActive(false);
            }
            else
            {
                spInven_Content[i % PageNumberlimit[key]].gameObject.SetActive(true);
                ActiveButton++;
                //Debug.Log(ItemDatabase.itemDatabase.item_list[i].Name);
                num = Sp_ItemEffect.sp_itemeffect.Sp_have[i];
                spInven_Content[i % PageNumberlimit[key]].GetComponent<Inven_Content_Sp>().ChangeAch(Sp_ItemDatabase.Sp_itemDatabase.Sp_item_all[num]);
                spInven_Content[i % PageNumberlimit[key]].GetComponent<Inven_Content_Sp>().item_forginkey = i;
            }
        }
    }

    public void FristStart()        //몇페이지 만들건지 체크
    {
        if (this.gameObject.activeSelf)
        {
            switch (key)
            {
                case 0:
                    SynergFrist();
                    break;
                case 1:
                    invenFrist();
                    break;
                case 2:
                    SpFrist();
                    break;
                default:
                    break;
            }
        }
    }

    public void invenFrist()
    {
        Inven_Content.Clear();
        Debug.Log("인벤첫");
        for (int i = 0; i < Inven_ContentPlace.transform.childCount; i++)
        {
            Inven_Content.Add(Inven_ContentPlace.transform.GetChild(i).gameObject);
        }
        PageTotal[key] = ItemDatabase.itemDatabase.item_list.Count / PageNumberlimit[key];
        if (ItemDatabase.itemDatabase.item_list.Count % PageNumberlimit[key] != 0)
        {
            PageTotal[key]++;
        }
    }

    public void SpFrist()
    {
        spInven_Content.Clear();
        Debug.Log("스피첫");
        for (int i = 0; i < SpInven_ContentPlace.transform.childCount; i++)
        {
            spInven_Content.Add(SpInven_ContentPlace.transform.GetChild(i).gameObject);
        }
        //수정
        PageTotal[key] = Sp_ItemEffect.sp_itemeffect.Sp_have.Count / PageNumberlimit[key];
        if (Sp_ItemEffect.sp_itemeffect.Sp_have.Count % PageNumberlimit[key] != 0)
        {
            PageTotal[key]++;
        }
    }
    public void SynergFrist()
    {
        synerg_Content.Clear();
        Debug.Log("시너지첫"+ Synergy_Manager.Sy_manager.Sy_total_List.Count+" / ");
        for (int i = 0; i < synerg_ContentPlace.transform.childCount; i++)
        {
            synerg_Content.Add(synerg_ContentPlace.transform.GetChild(i).gameObject);
        }
        //시너지 버튼 추가하면 넣기
        PageTotal[key] = Synergy_Manager.Sy_manager.Sy_total_List.Count / PageNumberlimit[key];
        if (Synergy_Manager.Sy_manager.Sy_total_List.Count % PageNumberlimit[key] != 0)
        {
            PageTotal[key]++;
        }
    }

    public void ButtonOn()
    {
        //R_Btn.onClick.RemoveAllListeners();
        //L_Btn.onClick.RemoveAllListeners();
        //ChangeElement_panel();
        //R_Btn.onClick.AddListener(R_Button);
        //L_Btn.onClick.AddListener(L_Button);
        ChangeElement_panel();
    }

    public void L_Button()
    {
        Debug.Log("타이틀 왼쪽 버튼 완");
        if (PageNow[key] > 1)
        {
            PageNow[key]--;
        }
        ChangeElement_panel();
        PageText.text = PageNow[key] + " / " + "3";
    }
    public void R_Button()
    {
        Debug.Log("타이틀 오른쪽 버튼 완");
        if (PageNow[key] < PageTotal[key])
        {
            PageNow[key]++;
        }
        ChangeElement_panel();
        PageText.text = PageNow[key] + " / " + "3";
    }

    public void SetOnButtonPage()
    {
        
        
        Total_Content[key].SetIsOnWithoutNotify(true);
        Total_Content[key].gameObject.GetComponent<OpenMyInven>().PageOn();
       
    }

}

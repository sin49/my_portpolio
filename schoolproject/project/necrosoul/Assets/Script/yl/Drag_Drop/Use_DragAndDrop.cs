using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Use_DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler //,IPointerClickHandler
{
    public Image data;
    public Item itemdata;
    public GameObject TrshItem;
    public DragAndDropContainer dragAndDropContainer;

    bool isDragging = false;


    // �巡�� ������Ʈ���� �߻�
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!this.gameObject.GetComponent<Slot>().NullPlace)
        {
            itemdata = this.gameObject.GetComponent<Slot>().item;
            Debug.Log(this.gameObject.name + "�巡�׽���");
            //Debug.Log(this.gameObject.name + "�̸�" + this.gameObject.GetComponent<Slot>().ReturnNumber());

            if (this.gameObject.GetComponent<Image>().sprite == null)
            {
                Debug.Log(this.gameObject.name + "���ư���!?");
                return;
            }
            Debug.Log(this.gameObject.name + "�巡�� ���� �Ϸ�");

            // Activate Container
            dragAndDropContainer.gameObject.SetActive(true);

            // Set Data 
            dragAndDropContainer.image.sprite = data.sprite;//this.gameObject.GetComponent<Image>().sprite;
            dragAndDropContainer.item = itemdata;
            dragAndDropContainer.Use = this.gameObject.GetComponent<Slot>().UseSlot;
            isDragging = true;
        }
    }
    // �巡�� ������Ʈ���� �߻�
    public void OnDrag(PointerEventData eventData)
    {
        if (!this.gameObject.GetComponent<Slot>().NullPlace)
        {
            Debug.Log(this.gameObject.name + "�巡����" + isDragging);
            if (isDragging)
            {
                dragAndDropContainer.transform.position = eventData.position;
            }
            else
            {
                return;
            }
        }
    }
    // �巡�� ������Ʈ���� �߻�        ������ �̺�Ʈ
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!this.gameObject.GetComponent<Slot>().NullPlace)
        {
            Debug.Log(this.gameObject.name + "�巡�׳�");
            //Debug.Log("�̸�"+data.sprite.name);
            if (isDragging)
            {
                if (dragAndDropContainer.image.sprite != null)
                {
                    // set data from dropped object  
                    data.sprite = dragAndDropContainer.image.sprite;
                    this.gameObject.GetComponent<Slot>().item = dragAndDropContainer.item;

                    if (this.gameObject.GetComponent<Slot>().UseSlot ^ dragAndDropContainer.Use)
                    {
                        Debug.Log("�ִ� �ڸ����� �̺�Ʈ �߻�");
                        this.gameObject.GetComponent<Slot>().UseAndItemChange();
                    }

                }
                else
                {
                    // Clear Data
                    data.sprite = null;
                    this.gameObject.GetComponent<Slot>().item = null;
                    itemdata = null;
                    Debug.Log("����ִ� ��");
                }

                this.gameObject.GetComponent<Slot>().FullCheckManger();
            }


            isDragging = false;
            // Reset Contatiner
            itemdata = null;
            dragAndDropContainer.image.sprite = null;
            dragAndDropContainer.item = null;
            dragAndDropContainer.gameObject.SetActive(false);
            dragAndDropContainer.Use = false;
        }
        
    }


    //--------------------------------------------------------------------------------

    // ��� ������Ʈ���� �߻�     //OnEndDrag���� ���� �߻���
    public void OnDrop(PointerEventData eventData)
    {
        if (!this.gameObject.GetComponent<Slot>().NullPlace)
        {
            Debug.Log(this.gameObject.name + "���");

            if (dragAndDropContainer.image.sprite != null)
            {
                // keep data instance for swap 
                Sprite tempSprite = data.sprite;
                Item tempitem = this.gameObject.GetComponent<Slot>().item;


                // set data from drag object on Container
                data.sprite = dragAndDropContainer.image.sprite;
                this.gameObject.GetComponent<Slot>().item = dragAndDropContainer.item;
                this.gameObject.GetComponent<Slot>().FullCheckManger();

                if (this.gameObject.GetComponent<Slot>().UseSlot ^ dragAndDropContainer.Use)
                {
                    this.gameObject.GetComponent<Slot>().UseAndItemChange();
                }
                Debug.Log("�޴� �ڸ����� �̺�Ʈ �߻�");
                // put data from drop object to Container. 
                dragAndDropContainer.image.sprite = tempSprite;
                dragAndDropContainer.item = tempitem;
                dragAndDropContainer.Use = this.gameObject.GetComponent<Slot>().UseSlot;
            }
            else
            {
                Debug.Log("�ƹ��͵� ������  ���� �ʾ�");
                dragAndDropContainer.image.sprite = null;
                dragAndDropContainer.item = null;
            }
        }
        else
        {
            if (dragAndDropContainer.image.sprite != null)
            {
                Debug.Log("������ �����ڽ��ϴ�."+ dragAndDropContainer.item.SlotNumber+"���� ��ũ��Ʈ"+this.name);

                if (dragAndDropContainer.item.SlotNumber >= 100)
                {

                    Debug.Log("�����մϴ�????");
                    Inventory.Use_InvenData.Remove(dragAndDropContainer.item);
                }
                else
                {
                    
                }
                this.gameObject.GetComponent<Slot>().item.parent.GetComponent<Slot>().item.Item_Useing=false;
                Debug.Log("�������� �����ϴ�.");
                isDragging = false;

                // Reset Contatiner
                dragAndDropContainer.image.sprite = null;
                dragAndDropContainer.item = null;
                dragAndDropContainer.gameObject.SetActive(false);
                dragAndDropContainer.Use = false;

                
            }
        }
    }

    public void ChangeSlotNumber()
    {
        
    }

    //public PointerEventData.InputButton btn1 = PointerEventData.InputButton.Left;
    //public PointerEventData.InputButton btn2 = PointerEventData.InputButton.Right;
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if(eventData.button==btn2)
    //    {

    //    }
    //}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UI_Inventory : MonoBehaviour
{
    public bool resizeUI = true;
    public bool visible = false; private bool visible_mem;
    public bool retestTheChildren = false;

    Animator[] anis = new Animator[0];
    public List<Image> orderedBox = new List<Image>();

    public Animator anima;

    [Header("UpperPart")]
    public List<RectTransform> herosPart;

    [Header("DownPart")]
    public GridLayoutGroup grid;
    public RectTransform secondCouche;

    [Header("Validation")]
    public bool validation_on = false;
    public RectTransform val_element;
    public float val_OpenSpeed = 4f;
    public List<Button> val_toTurnOn;

    // Start is called before the first frame update
    void Start()
    {
        Resize();
        visible_mem = visible;
        if(Application.isPlaying)
            GetComponent<CanvasGroup>().alpha = 1;

        FillAnimAndSpriteBox();
    }

    public void FillAnimAndSpriteBox()
    {
        anis = grid.transform.GetComponentsInChildren<Animator>();
        
        orderedBox = new List<Image>();
        foreach (Animator ani in anis)
        {
            string number = ani.name.Remove(0, "InventoryCase (".Length).Replace(")", "");
            int.TryParse(number, out int res);

            Image sR_Box = ani.transform.GetChild(1).GetComponent<Image>();
            
            orderedBox.Add(sR_Box);

            continue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                validation_on = !visible;
                if (visible)
                    CloseInventory();
                else
                    OpenInventory();
            }
        }

        if(resizeUI)
        {
            Resize();
            resizeUI = false;
        }

        if (visible_mem != visible)
        {
            WaveBoxInv(visible);
            visible_mem = visible;
        }

        if (retestTheChildren)
        {
            anis = null;
            FillAnimAndSpriteBox();            
            retestTheChildren = false;
        }

        UpdateValidation();
    }
    
    public void UpdateValidation()
    {
        if (!Application.isPlaying)
            return;
        val_element.rotation = Quaternion.Lerp(val_element.rotation, Quaternion.Euler((validation_on ? 0 : 90), 0, 0), Time.deltaTime * val_OpenSpeed);
    }

    public void OpenInventory()
    {
        Resize();
        anima.SetBool("Visible", true);
        visible = true;
        WaveBoxInv(true);
        visible_mem = true;

        Debug.Log("Let's go :");
        UpdateInvVisual();
    }

    public void CloseInventory()
    {
        Resize();
        anima.SetBool("Visible", false);
        visible = false;
        WaveBoxInv(false);
        visible_mem = false;

        Vaisseau.instance.ClosedInventory();
    }

    public float delayBetween = 0.05f;
    public void WaveBoxInv(bool visible)
    {
        foreach (Animator ani in anis)
        {
//            Debug.Log("Yes yes");
            string number = ani.name.Remove(0,"InventoryCase (".Length).Replace(")", "");
            int.TryParse(number, out int res);
            float toWait = (res % 10) + ((int)res / 10);
            toWait *= delayBetween;
//Debug.Log("toWait = " + toWait + "res = " + number + " res = " + res);
            StartCoroutine(LaunchAnim(ani, toWait, visible));
        }
    }
    public IEnumerator LaunchAnim( Animator ani, float timer, bool visible)
    {
        yield return new WaitForSeconds(timer);
        ani.SetBool("Dis", !visible);
    }
    
    public bool forceUpdateInv = false;
    public void UpdateInvVisual()
    {
        Dictionary<item, int> inv = Vaisseau.instance.inventory;

        int index = 0;
        foreach (KeyValuePair<item,int> pair in inv)
        {
            orderedBox[index].GetComponent<Item_Draggable>().itemHere = pair.Key;
            orderedBox[index].sprite = pair.Key.forInv;
            orderedBox[index].color = pair.Key.color;
            index++;
        }
        for (int i = index; i < orderedBox.Count; i++)
        {
            orderedBox[i].sprite = null;
            orderedBox[i].color = Color.clear;
        }
    }


    public void Resize()
    {
        Debug.Log(secondCouche.rect.size);

        Vector2 size = secondCouche.rect.size;
        Vector2 spacing = new Vector2(secondCouche.rect.size.x/111, secondCouche.rect.size.y/34);
        size = spacing * 10;
        grid.cellSize = size;
        grid.padding = new RectOffset((int)spacing.x, (int)spacing.x, (int)spacing.y, (int)spacing.y);
        grid.spacing = new Vector2((int)spacing.x, (int)spacing.y);
    }
}

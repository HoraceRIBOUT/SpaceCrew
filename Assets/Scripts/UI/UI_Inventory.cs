using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UI_Inventory : MonoBehaviour
{
    public bool resizeUI = true;
    public bool visible = false; private bool visible_mem;


    public Animator anima;

    [Header("UpperPart")]
    public List<RectTransform> herosPart;

    [Header("DownPart")]
    public GridLayoutGroup grid;
    public RectTransform secondCouche;

    // Start is called before the first frame update
    void Start()
    {
        Resize();
        visible_mem = visible;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                anima.SetBool("Visible", !anima.GetBool("Visible"));
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
        
    }

    public float delayBetween = 0.05f;
    public void WaveBoxInv(bool visible)
    {
        Animator[] anis = grid.transform.GetComponentsInChildren<Animator>();
        foreach (Animator ani in anis)
        {
            string number = ani.name.Remove(0,"InventoryCase (".Length).Replace(")", "");
            int.TryParse(number, out int res);
            float toWait = (res % 10) + ((int)res / 10);
            toWait *= delayBetween;
            Debug.Log("toWait = " + toWait + "res = " + number + " res = " + res);
            StartCoroutine(LaunchAnim(ani, toWait, visible));
        }
    }
    public IEnumerator LaunchAnim( Animator ani, float timer, bool visible)
    {
        yield return new WaitForSeconds(timer);
        ani.SetBool("Dis", !visible);
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

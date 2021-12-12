using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FontSizeRatio : MonoBehaviour
{
    [Header("☟ ")]
    [Tooltip("If not, is ratio from the whole screen")]
    public bool ratioFromTheRect;
    [Range(0,1)]
    public float ratio = 0.33f;

    private Vector2 screenSize;

    public RectTransform myRectTransform;
    public Text myText;
    public TMP_Text myTMP_Text;

    public bool resize = false;


    // Start is called before the first frame update
    void Start()
    {
        if(myRectTransform == null)
        {
            myRectTransform = this.GetComponent<RectTransform>();
        }

        if (myText == null && myTMP_Text == null)
        {
            myText = this.GetComponent<Text>();
            myTMP_Text = this.GetComponent<TMP_Text>();

        }

        Resize();
        screenSize = new Vector2(Screen.width, Screen.height);
    }
    
    private void LateUpdate()
    {
        if(screenSize.x != Screen.width || screenSize.y != Screen.height)
        {
            Resize();
            screenSize = new Vector2(Screen.width, Screen.height);
        }
    }

    public void Update()
    {
        if (resize)
        {
            Resize();
            resize = false;
        }
    }

    void Resize()
    {
        if (ratioFromTheRect)
        {
            if (myText != null)
            {
                myText.fontSize =(int) (ratio * myRectTransform.rect.size.y);
            }
            if (myTMP_Text != null)
            {
                myTMP_Text.fontSize = ratio * myRectTransform.rect.size.y;
            }
        }
        else
        {
            if (myText != null)
            {
                myText.fontSize = (int)(ratio * Screen.height);
            }
            if (myTMP_Text != null)
            {
                myTMP_Text.fontSize = ratio * Screen.height;
            }
        }
    }
}

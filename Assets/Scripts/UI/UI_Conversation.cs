using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UI_Conversation : MonoBehaviour
{
    public Conversation currentConversationDisplayed;

    public bool displayIt = false;
    public bool displayNext = false;

    public bool on = false;
    public bool canPassToNext = false;


    [Header("Part")]
    public GameObject icon;
    public Image iconMain;
    public Animator iconMainAnim;
    public Image iconFace;
    public Image iconShad;
    public Image iconBg; //color change between frame

    public TMPro.TMP_Text textToFill;
    public Animator convAnim;
    public Animator iconClick;

    [Header("Element")]
    public List<Sprite> characterSprite;
    public List<Sprite> characterShadowSprite;
    public List<Sprite> facePerPilot;
    public List<Sprite> facePerMilit;
    public List<Sprite> facePerMecan;

    public string plhold = "Ooooo!";

    public Coroutine textAppear = null;

    // Start is called before the first frame update
    void Start()
    {
        if(Application.isPlaying)
            GetComponent<CanvasGroup>().alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            if ((Input.GetMouseButtonDown(0) || 
                Input.GetKeyDown(KeyCode.Space) 
                ) && canPassToNext)
            {
                if (ConversationAdvance())
                {
                    DisplayConversation(currentConversationDisplayed);
                }
                else
                {
                    //ConversationAdvance do the "Finish();"
                }
            }
        }

        
        if (displayNext)
        {
            if (ConversationAdvance())
            {
                DisplayConversation(currentConversationDisplayed);
            }
            displayNext = false;
        }
        if (displayIt)
        {
            DisplayConversation(currentConversationDisplayed);
            displayIt = false;
        }
    }

    public void StartThisConversation(Conversation conv)
    {
        currentConversationDisplayed = conv;
        conv.currentIndex = 0;
        DisplayConversation(conv);
    }

    void DisplayConversation(Conversation currentConversationDisplayed)
    {
        Sentence sent = currentConversationDisplayed.sentences[currentConversationDisplayed.currentIndex];
        Debug.Log("Update conversation (" + currentConversationDisplayed.currentIndex + ") : " + sent.textDisplay);
        int indexCharac = (int)sent.character - 1;
        convAnim.SetBool("Icon", (indexCharac != -1));
        convAnim.SetBool("Visible", true);
        on = true;
        if (indexCharac != -1)
        {
            Debug.Log("indexCharac " + indexCharac);
            iconMain.sprite = characterSprite[indexCharac];
            if (iconMainAnim != null)
                iconMainAnim.SetInteger("State", sent.animation);
            iconShad.sprite = characterShadowSprite[indexCharac];
            switch (sent.character)
            {
                case Character.pilot:
                    iconFace.sprite = facePerPilot[sent.indexFace];
                    break;
                case Character.milit:
                    iconFace.sprite = facePerMilit[sent.indexFace];
                    break;
                case Character.mecan:
                    iconFace.sprite = facePerMecan[sent.indexFace];
                    break;
                case Character.alien:
                    Debug.Log("No face");
                    break;
                case Character.None: break;
                default: break;
            }
        }
        iconBg.color = sent.bgColor;

        if (!Application.isPlaying)
        {
            textToFill.text = ((indexCharac != -1) ? plhold : "") + sent.textDisplay;
            return;
        }

        if(textAppear != null)
        {
            StopCoroutine(textAppear);
        }
        canPassToNext = false;
        if(iconClick != null)
            iconClick.SetBool("On",false);
//        Debug.Log("until here");
        textAppear = StartCoroutine(TextApproch(sent, (indexCharac != -1)));
    }

    float defaultSpeed = 40f;

    public IEnumerator TextApproch(Sentence sent, bool phold)
    {
        float timeBetween = 1f / (sent.speed * defaultSpeed );
        string textDisplay = sent.textDisplay;
        //Debug.Log("until here : "+ textDisplay.Length);
        for (int i = 0; i < textDisplay.Length; i++)
        {
            string s = (phold ? plhold : "");
            //Debug.Log("i " + i + " / "+ textDisplay.Length 
            //    + "\n" + textDisplay);
            s += textDisplay.Remove(i, textDisplay.Length - i);
            s += "<color=#FFFFFF00>";
            s += textDisplay.Remove(0, i);
            s += "</color>";
            textToFill.text = s;

            if (i > 0 && Input.GetMouseButtonDown(0))
            {
                break;
            }

            
            yield return new WaitForSeconds(timeBetween);
            if (timeBetween < Time.deltaTime)
            {
//                Debug.Log("Time.deltaTime " + Time.deltaTime + " / timeBetween " + timeBetween + " = " + (Time.deltaTime / timeBetween));
                i += (int)(Time.deltaTime / timeBetween) - 1;
            }
        }
        textToFill.text = (phold ? plhold : "") + textDisplay;
        textAppear = null;
        canPassToNext = true;
        if (iconClick != null)
            iconClick.SetBool("On", true);
    }


    public bool ConversationAdvance()
    {
        currentConversationDisplayed.currentIndex++;
        if (currentConversationDisplayed.currentIndex >= currentConversationDisplayed.sentences.Count)
        {
            Debug.Log("Finish here !");
            Finish();
            currentConversationDisplayed.currentIndex = 0;
            return false;
        }
        return true;
    }

    public void Finish()
    {
        convAnim.SetBool("Visible", false); //maybe ?
        on = false;
        
        canPassToNext = false;
        if (iconClick != null)
            iconClick.SetBool("On", false);
    }
}

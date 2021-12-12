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


    [Header("Part")]
    public GameObject icon;
    public Image iconMain;
    public Image iconFace;
    public Image iconShad;
    public Image iconBg; //color change between frame

    public TMPro.TMP_Text textToFill;
    public Animator convAnim;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (displayNext)
        {
            ConversationAdvance();
            DisplayConversation(currentConversationDisplayed);
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
        int indexCharac = (int)sent.character - 1;
        convAnim.SetBool("Icon", (indexCharac != -1));
        convAnim.SetBool("Visible", true);
        if(indexCharac != -1)
        {
            Debug.Log("indexCharac " + indexCharac);
            iconMain.sprite = characterSprite[indexCharac];
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
        textAppear = StartCoroutine(TextApproch(sent, (indexCharac != -1)));
    }

    float defaultSpeed = 40f;

    public IEnumerator TextApproch(Sentence sent, bool phold)
    {
        float timeBetween = 1f / (sent.speed * defaultSpeed );
        string textDisplay = sent.textDisplay;
        for (int i = 0; i < textDisplay.Length; i++)
        {
            string s = (phold ? plhold : "");
            Debug.Log("i " + i + " / "+ textDisplay.Length 
                + "\n" + textDisplay);
            s += textDisplay.Remove(i, textDisplay.Length - i);
            s += "<color=#FFFFFF00>";
            s += textDisplay.Remove(0, i);
            s += "</color>";
            textToFill.text = s;

            yield return new WaitForSeconds(timeBetween);
            if (timeBetween < Time.deltaTime)
            {
                Debug.Log("Time.deltaTime " + Time.deltaTime + " / timeBetween " + timeBetween + " = " + (Time.deltaTime / timeBetween));
                i += (int)(Time.deltaTime / timeBetween) - 1;
            }
        }
        textToFill.text = (phold ? plhold : "") + textDisplay;
        textAppear = null;
    }


    public void ConversationAdvance()
    {
        currentConversationDisplayed.currentIndex++;
        if (currentConversationDisplayed.currentIndex >= currentConversationDisplayed.sentences.Count)
        {
            convAnim.SetBool("Visible", false); //maybe ?
            currentConversationDisplayed.currentIndex = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Clique gauche : fire : later more turret (bomb / many / en croix) ---> surchauffe ?
//Cloque droit : dash ? with aptitude ? Like invicibilities ? More hitbox ?

//Go to planet to land and then, upgrade ! It's that good ?

public class Vaisseau : MonoBehaviour
{
    public static Vaisseau instance = null;
    public void Awake()
    {
        instance = this;
    }

    [Header("Part")]
    public Camera mainCam;
    public UI_Manager ui_man;

    [Header("Defense")]
    public float pv = 100;
    public float pvMax = 100;
    public float armor = 0;
    public float regenPerSeconds = 0;

    [Header("Movement")]
    public float speedGainPerSecond = 0.1f;
    public float friction = 0.05f;
    public float maxSpeed = 3f;

    public Vector3 currentSpeed;


    [Header("Shoot")]
    public List<Turret> turrets;
    public float damageMult = 1;
    public float shotSizeMult = 1;
    public float contactDamage = 0;

    public float surchauffe = 0;
    public float surchauffeMax = 2;
    private bool surchauffeOn = false;

    [Header("Inventory")]
    public List<item> inventory_forInsp = new List<item>();
    public Dictionary<item, int> inventory = new Dictionary<item, int>();

    [Header("Chara")]
    public Dictionary<Character, Rom_Stat> romStatPerChara = new Dictionary<Character, Rom_Stat>();

    [Header("Visual")]
    public Transform visualMain;
    public SpriteRenderer surchauffeRenderer;
    public Gradient colorSurchauffe;
    public UnityEngine.UI.Slider surchauffeSlid;
    public TMPro.TMP_Text surchauffeText;

    public UnityEngine.UI.Slider pvSlid;
    public TMPro.TMP_Text pvText;

    public ConvData convData;

    [Header("Tuto")]
    public TMPro.TMP_Text tutoMove;
    public TMPro.TMP_Text tutoShoot;
    public TMPro.TMP_Text tutoShootAndClick;
    public bool moveDOne;
    public bool shootDone;
    public bool gatherRessourceDone;

    // Start is called before the first frame update
    void Start()
    {
        if (friction >= speedGainPerSecond)
            friction = speedGainPerSecond / 2f;

        convData.Initialize();

        romStatPerChara.Add(Character.milit, new Rom_Stat());
        romStatPerChara.Add(Character.pilot, new Rom_Stat());
        romStatPerChara.Add(Character.mecan, new Rom_Stat());

        StartCoroutine(CinematicIntro());
    }

    public IEnumerator CinematicIntro()
    {
        //Logo disapear

        yield return new WaitForSeconds(0.1f);
        ui_man.conversation.StartThisConversation(convData.entranceConv);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ui_man.conversation.on)
        {
            MovementManager();

            ShootManagement();
        }

        if(pv < pvMax)
        {
            pv+= regenPerSeconds*Time.deltaTime;
        }

        UpdateVisual();

        TutorialManagement();
    }


    public void MovementManager()
    {
        if (landingOn)
        {
            Landing();
            return;
        }


        Vector2 direction = Vector2.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        direction.Normalize();

        Vector3 speedGain = direction * speedGainPerSecond;
        currentSpeed += speedGain;
        currentSpeed -= currentSpeed.normalized * friction;

        if (currentSpeed.sqrMagnitude > maxSpeed * maxSpeed)
            currentSpeed = currentSpeed.normalized * maxSpeed;
        
        this.transform.position += currentSpeed * Time.deltaTime;
    }

    public void UpdateVisual()
    {
        Vector3 currentWorldForwardDirection = transform.TransformDirection(Vector3.up);
        float angleDiff = Vector3.SignedAngle(currentWorldForwardDirection, currentSpeed.normalized, Vector3.forward);

        if (currentSpeed.sqrMagnitude > 0.05f)
            visualMain.rotation = Quaternion.Euler(Vector3.forward * angleDiff);
        //visualMain.rotation = ;

        surchauffeRenderer.color = colorSurchauffe.Evaluate((surchauffe / surchauffeMax) * (surchauffe / surchauffeMax));


        pvSlid.value = Mathf.Lerp(pvSlid.value, pv / pvMax, Time.deltaTime);
        pvText.text = pv + "/" + pvMax;

        surchauffeSlid.value = surchauffe / surchauffeMax;
        if(surchauffeOn)
            surchauffeText.text = "Overheat";
        else
            surchauffeText.text = "";
    }


    
    public void ShootManagement()
    {
        if (landingOn)
            return;

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = this.transform.position.z;

        if (surchauffe > 0)
            surchauffe -= Time.deltaTime;

        if (surchauffe > surchauffeMax)
            surchauffeOn = true;

        if (surchauffeOn)
        {
            if(surchauffe > 0)
                return;
            surchauffeOn = false;
        }
        

        foreach (Turret turr in turrets)
        {
            if(Input.GetMouseButton(0))
                surchauffe += turr.Try_Shoot(mousePos, damageMult, shotSizeMult);
            
            turr.UpdateVisual(mousePos);
        }
    }



    public void ApplyItem(item itemToApply, Character chara)
    {
        switch (chara)
        {
            case Character.pilot:
                romStatPerChara[chara] += itemToApply.romStatToAddToPilot;
                AddStat(itemToApply.statToAddPilot);

                break;
            case Character.milit:
                romStatPerChara[chara] += itemToApply.romStatToAddToMilit;
                AddStat(itemToApply.statToAddMilit);

                break;
            case Character.mecan:
                romStatPerChara[chara] += itemToApply.romStatToAddToMecan;
                AddStat(itemToApply.statToAddMecan);

                break;
            case Character.None:
            case Character.alien:
            default:
                break;
        }

        UpdateLandConvList();
    }

    public void UpdateLandConvList()
    {
        convData.UpdateLandConvList();
    }

    public void AddInInv(item itemAdd)
    {
        Debug.Log("TO DO : just imagine you have an inventory with now " + itemAdd.type + " in it.");
        if (!inventory.ContainsKey(itemAdd))
        {
            inventory.Add(itemAdd, 1);
            inventory_forInsp.Add(itemAdd);
        }
        else
        {
            Debug.Log("already one version in it so add it !");
            inventory[itemAdd] = inventory[itemAdd] + 1;
            inventory_forInsp.Add(itemAdd);
        }

        //Add a fx in ui to make sure that player know that he have collect an item

        if (!gatherRessourceDone)
        {
            gatherRessourceDone = true;
            if (moveDOne && shootDone && gatherRessourceDone)
                ui_man.conversation.StartThisConversation(convData.afterTutoConv);
        }
    }
    public void RemInInv(item itemAdd, int numberToRem = 1)
    {
        if (!inventory.ContainsKey(itemAdd))
        {
            Debug.LogError("Try to remove an item not in inventory!!!");
        }
        else
        {
            if (inventory[itemAdd] <= numberToRem)
            {
                inventory.Remove(itemAdd);
                Debug.Log("Remove all " + itemAdd.type + " (" + numberToRem + ")");
            }
            else
            {
                inventory[itemAdd] = inventory[itemAdd] - numberToRem;
                Debug.Log("Remove " + itemAdd.type + " rest : " + inventory[itemAdd]);
            }

        }

        //Add a fx in ui to make sure that player know that he have collect an item
    }


    public void TutorialManagement()
    {
        if(!moveDOne || !shootDone)
        {
            if (currentSpeed.magnitude == maxSpeed)
                moveDOne = true;

            if (Input.GetMouseButtonDown(0) && (!ui_man.conversation.on) && tutoShoot.alpha > 0.5)
                shootDone = true;

            if (moveDOne && shootDone && gatherRessourceDone)
                ui_man.conversation.StartThisConversation(convData.afterTutoConv);
        }

        if (moveDOne && tutoMove.alpha > 0)
            tutoMove.alpha -= Time.deltaTime * 0.5f;
        else if (!moveDOne && (!ui_man.conversation.on) && tutoMove.alpha < 1)
            tutoMove.alpha += Time.deltaTime * 0.5f;

        if (shootDone && tutoShoot.alpha > 0)
        {
            tutoShoot.alpha -= Time.deltaTime * 0.5f;
            tutoShootAndClick.alpha -= Time.deltaTime * 0.5f;
        }
        else if(!shootDone && (!ui_man.conversation.on) && tutoShoot.alpha < 1)
        {
            tutoShoot.alpha += Time.deltaTime * 0.5f;
            tutoShootAndClick.alpha += Time.deltaTime * 0.5f;
        }

    }

    public void AddStat(Stat statToAdd)
    {
        Stat pastStat = getStat();

        maxSpeed += statToAdd.speedMax;
        speedGainPerSecond += statToAdd.speedGain;
        friction += statToAdd.friction;
        damageMult += statToAdd.damage;
        shotSizeMult += statToAdd.shotSize;
        pv += statToAdd.pv;
        pv += statToAdd.pvMax;
        armor += statToAdd.armor;
        surchauffeMax += statToAdd.surchauffeMax;
        regenPerSeconds += statToAdd.regen;
        contactDamage += statToAdd.contactDamage;

        ui_man.inventory.vaisseauStat.VisualStat(pastStat, getStat());
    }




    #region collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collectable collect = collision.GetComponent<Collectable>();
        if (collect != null)
        {
            Collision_Collect(collect);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Planet planet = collision.gameObject.GetComponent<Planet>();
        if (planet != null)
        {
            Collision_Planet(planet);
        }
    }

    public void Collision_Collect(Collectable collect)
    {
        AddInInv(collect.itemCollect);
        collect.Death();
    }


    public void Collision_Planet(Planet planet)
    {
        Debug.Log("Land.");

        landingPoint = planet.transform.position + (this.transform.position - planet.transform.position).normalized * (planet.size + landingOffset);
        landingPoint_start = this.transform.position;
        float angle = Vector3.SignedAngle(Vector3.up, (this.transform.position - planet.transform.position).normalized, Vector3.forward);
        Debug.Log("Angle = " + angle);
        landingRotation = Quaternion.Euler(0,0, angle);
        landingRotation_start = visualMain.rotation;
        currentSpeed = Vector3.zero;
        landingLerp = 0;

        landingOn = true;
    }

    [Header("Landing")]
    public bool landingOn;
    public Vector3 landingPoint;
    private Vector3 landingPoint_start;
    public float landingOffset = 0.2f;
    public Quaternion landingRotation;
    private Quaternion landingRotation_start;
    private float landingLerp = 0;
    public float landingSpeed = 0.5f;
    public AnimationCurve landingPosCurve;
    public AnimationCurve landingRotCurve;

    public void Landing()
    {
        if (landingLerp >= 1)
        {
            landingLerp = 1;
        }
        else
        {
            landingLerp += Time.deltaTime * landingSpeed;
            landingLerp = Mathf.Clamp01(landingLerp);
            if (landingLerp == 1)
                Land();
        }
        visualMain.rotation = Quaternion.Lerp(landingRotation_start, landingRotation, landingRotCurve.Evaluate(landingLerp));
        this.transform.position = Vector3.Lerp(landingPoint_start, landingPoint, landingPosCurve.Evaluate(landingLerp));
    }

    private Conversation lastConvLand = null;
    public void Land()
    {
        pv = pvMax;
        //if conversation : make conv before landing
        if(convData.landingConv.Count != 0)
        {
            if(convData.landingConv.Count == 1)
            {
                lastConvLand = convData.landingConv[0];
                lastConvLand.actionToPerformAtEnd += OpenInventory;
                ui_man.conversation.StartThisConversation(lastConvLand);
                return;
            }
            int randomRange = Random.Range(0, 100);
            if(randomRange > 50)
            {
                int index = Random.Range(0, convData.landingConv.Count - 1);
                lastConvLand = convData.landingConv[index];
                lastConvLand.actionToPerformAtEnd += OpenInventory;
                ui_man.conversation.StartThisConversation(lastConvLand);
                return;
            }
        }

        OpenInventory();

    }

    public void OpenInventory()
    {
        lastConvLand.actionToPerformAtEnd = null;
        ui_man.inventory.OpenInventory();
    }

    public Stat getStat()
    {
        Stat res = new Stat();
        res.speedMax        = maxSpeed;
        res.speedGain       = speedGainPerSecond;
        res.friction        = friction;
        res.damage          = damageMult;
        res.pv              = pv;
        res.pvMax           = pvMax;
        res.armor           = armor;
        res.surchauffeMax   = surchauffeMax;
        res.regen           = regenPerSeconds;
        res.contactDamage   = contactDamage;
        res.shotSize        = shotSizeMult;

        return res;
    }                   

    public void ClosedInventory()
    {
        landingOn = false;
    }



    #endregion
}

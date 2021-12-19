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
    public float armor = 0;

    [Header("Movement")]
    public float speedGainPerSecond = 0.1f;
    public float friction = 0.05f;
    public float maxSpeed = 3f;

    public Vector3 currentSpeed;


    [Header("Shoot")]
    public List<Turret> turrets;
    public float damageMult = 1;

    public float surchauffe = 0;
    public float surchauffeMax = 2;
    private bool surchauffeOn = false;

    [Header("Inventory")]
    public List<item> inventory_forInsp = new List<item>();
    public Dictionary<item, int> inventory = new Dictionary<item, int>();

    [Header("Visual")]
    public Transform visualMain;
    public SpriteRenderer surchauffeRenderer;
    public Gradient colorSurchauffe;

    public ConvData convData;

    // Start is called before the first frame update
    void Start()
    {
        if (friction >= speedGainPerSecond)
            friction = speedGainPerSecond / 2f;

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
        UpdateVisual();
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
                surchauffe += turr.Try_Shoot(mousePos, damageMult);
            
            turr.UpdateVisual(mousePos);
        }
    }


    public void AddStat(Stat statToAdd)
    {
        maxSpeed += statToAdd.speedMax;
        speedGainPerSecond += statToAdd.speedGain;
        friction += statToAdd.friction;
        damageMult += statToAdd.damage;
        pv += statToAdd.pv;
        armor += statToAdd.armor;
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
    }
    public void RemInInv(item itemAdd, int numberToRem = 1)
    {
        if (!inventory.ContainsKey(itemAdd))
        {
            Debug.LogError("Try to remove an item not in inventory!!!");
        }
        else
        {
            if (inventory[itemAdd] == 1)
            {
                inventory.Remove(itemAdd);
            }
            else
            {
                inventory[itemAdd] = inventory[itemAdd] - numberToRem;
            }
            
        }

        //Add a fx in ui to make sure that player know that he have collect an item
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

    public void Land()
    {
        //if conversation : make conv before landing
        OpenInventory();
    }

    public void OpenInventory()
    {
        ui_man.inventory.OpenInventory();
    }

    public void ClosedInventory()
    {
        landingOn = false;
    }



    #endregion
}

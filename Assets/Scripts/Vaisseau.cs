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

    [Header("Visual")]
    public Transform visualMain;



    // Start is called before the first frame update
    void Start()
    {
        if (friction >= speedGainPerSecond)
            friction = speedGainPerSecond / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        MovementManager();

        ShootManagement();
    }

    public void MovementManager()
    {
        Vector2 direction = Vector2.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        direction.Normalize();

        Vector3 speedGain = direction * speedGainPerSecond;
        currentSpeed += speedGain;
        currentSpeed -= currentSpeed.normalized * friction;

        if (currentSpeed.sqrMagnitude > maxSpeed * maxSpeed)
            currentSpeed = currentSpeed.normalized * maxSpeed;
        
        UpdateVisual();

        this.transform.position += currentSpeed * Time.deltaTime;
    }

    public void UpdateVisual()
    {
        Vector3 currentWorldForwardDirection = transform.TransformDirection(Vector3.up);
        float angleDiff = Vector3.SignedAngle(currentWorldForwardDirection, currentSpeed.normalized, Vector3.forward);

        if (currentSpeed.sqrMagnitude > 0.05f)
            visualMain.rotation = Quaternion.Euler(Vector3.forward * angleDiff);
        //visualMain.rotation = ;

    }

    public void ShootManagement()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = this.transform.position.z;

        foreach (Turret turr in turrets)
        {
            if(Input.GetMouseButton(0))
                turr.Try_Shoot(mousePos, damageMult);
            
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
            if (collect.itemType == ItemCollectable.None)
            {
                AddStat(collect.statToAdd);
                collect.Death();
            }
            else
            {
                //Add to inventory (with UI effect and all ! It will be glorious)
                Debug.Log("TO DO : just imagine you have an inventory with now " + collect.itemType + " in it.");
                collect.Death();
            }
        }
    }

    #endregion
}

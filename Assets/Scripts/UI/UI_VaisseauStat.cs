using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UI_VaisseauStat : MonoBehaviour
{
    
    [HorizontalGroup("1")]public Slider sliderNew_speedMax;     [HorizontalGroup("1")]public Slider sliderOld_speedMax;      public float valueMax_speedMax;
    [HorizontalGroup("2")]public Slider sliderNew_speedGain;    [HorizontalGroup("2")]public Slider sliderOld_speedGain;     public float valueMax_speedGain;
    [HorizontalGroup("3")]public Slider sliderNew_friction;     [HorizontalGroup("3")]public Slider sliderOld_friction;      public float valueMax_friction;
    [HorizontalGroup("4")]public Slider sliderNew_damage;       [HorizontalGroup("4")]public Slider sliderOld_damage;        public float valueMax_damage;
    [HorizontalGroup("5")]public Slider sliderNew_pvMax;        [HorizontalGroup("5")]public Slider sliderOld_pvMax;         public float valueMax_pvMax;
    [HorizontalGroup("6")]public Slider sliderNew_armor;        [HorizontalGroup("6")]public Slider sliderOld_armor;         public float valueMax_armor;
    [HorizontalGroup("7")]public Slider sliderNew_surchauffeMax;[HorizontalGroup("7")]public Slider sliderOld_surchauffeMax; public float valueMax_surchauffeMax;
    public CanvasGroup newStat;
    [HorizontalGroup("8")]public Slider sliderNew_regen;        [HorizontalGroup("8")]public Slider sliderOld_regen;         public float valueMax_regen;
    [HorizontalGroup("9")]public Slider sliderNew_contactDamage;[HorizontalGroup("9")]public Slider sliderOld_contactDamage; public float valueMax_contactDamage;
    [HorizontalGroup("10")]public Slider sliderNew_shotSize;    [HorizontalGroup("10")]public Slider sliderOld_shotSize;     public float valueMax_shotSize;

    //Potential :
    //Contact damage
    //Shot size
    //Regen

    //
    public float speed = 0.15f;

    public Stat currentStat;
    public Stat targetStat;

    public void VisualStat(Stat oldStat, Stat newStat)
    {
        currentStat = oldStat;
        targetStat = newStat;

    }

    public void UpdateStatVisu()
    {
        sliderNew_speedMax       .value = targetStat.speedMax         / valueMax_speedMax     ;
        sliderNew_speedGain      .value = targetStat.speedGain        / valueMax_speedGain    ;
        sliderNew_friction       .value = targetStat.friction         / valueMax_friction     ;
        sliderNew_damage         .value = targetStat.damage           / valueMax_damage       ;
        sliderNew_pvMax          .value = targetStat.pvMax            / valueMax_pvMax        ;
        sliderNew_armor          .value = targetStat.armor            / valueMax_armor        ;
        sliderNew_surchauffeMax  .value = targetStat.surchauffeMax    / valueMax_surchauffeMax;
        sliderNew_regen          .value = targetStat.regen            / valueMax_regen        ;
        sliderNew_contactDamage  .value = targetStat.contactDamage    / valueMax_contactDamage;
        sliderNew_shotSize       .value = targetStat.shotSize         / valueMax_shotSize     ;

        
        sliderOld_speedMax       .value = currentStat.speedMax         / valueMax_speedMax     ;
        sliderOld_speedGain      .value = currentStat.speedGain        / valueMax_speedGain    ;
        sliderOld_friction       .value = currentStat.friction         / valueMax_friction     ;
        sliderOld_damage         .value = currentStat.damage           / valueMax_damage       ;
        sliderOld_pvMax          .value = currentStat.pvMax            / valueMax_pvMax        ;
        sliderOld_armor          .value = currentStat.armor            / valueMax_armor        ;
        sliderOld_surchauffeMax  .value = currentStat.surchauffeMax    / valueMax_surchauffeMax;
        sliderOld_regen          .value = currentStat.regen            / valueMax_regen        ;
        sliderOld_contactDamage  .value = currentStat.contactDamage    / valueMax_contactDamage;
        sliderOld_shotSize       .value = currentStat.shotSize         / valueMax_shotSize     ;


        if (newStat.alpha < 1               && (
            targetStat.regen != 0           ||
            targetStat.contactDamage != 0   ||
            targetStat.shotSize != 1
            ))
        {
            Debug.Log("Wait why  " + newStat.alpha + " | " +
                                     targetStat.regen + " | " +
                                     targetStat.contactDamage + " | " +
                                     targetStat.shotSize + " | ");
            
            newStat.alpha += Time.deltaTime;
        }

    }

    public void Update()
    {
        currentStat = Stat.Lerp(currentStat, targetStat, Time.deltaTime * speed);
        UpdateStatVisu();
    }




}

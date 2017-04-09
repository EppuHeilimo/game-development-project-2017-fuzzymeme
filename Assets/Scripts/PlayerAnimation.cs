using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

#endif

using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum WeaponType
    {
        Melee = 0,
        OneHanded,
        TwoHanded
    }
    private Animator anim;
    public float inputHorizontal = 0;
    public float inputVertical = 0;
    public int quadrant;
    public WeaponType wepType;
    
    // Use this for initialization
    void Start ()
	{
        anim = transform.FindChild("girl").GetComponent<Animator>();
        wepType = WeaponType.Melee;
    }
	
	// Update is called once per frame
	void Update () {

	    float angle = transform.localEulerAngles.y;
        //looking up
	    if (angle >= 315 || angle < 45)
	    {
            anim.SetFloat("Vertical", inputVertical);
            anim.SetFloat("Horizontal", inputHorizontal);
        }
        else if (angle >= 45 && angle < 135)
        {
            anim.SetFloat("Vertical", inputHorizontal);
            anim.SetFloat("Horizontal", inputVertical);
        }
        else if (angle >= 135 && angle < 225)
        {
            anim.SetFloat("Vertical", -1 * inputVertical);
            anim.SetFloat("Horizontal", -1 * inputHorizontal);
        }
        else if (angle >= 225 && angle < 315)
        {
            anim.SetFloat("Vertical", -1 * inputHorizontal);
            anim.SetFloat("Horizontal", -1 * inputVertical);
        }

        

    }

    public void AnimateSlash()
    {
        
        anim.Play("Slash", 1);
    }


    public void SetWeaponType(WeaponType type)
    {
        wepType = type;
        anim.SetInteger("WeaponType", (int)type);
    }
}

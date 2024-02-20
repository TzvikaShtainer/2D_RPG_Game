using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Update = Unity.VisualScripting.Update;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Spin Info")]
    [SerializeField] private UI_SkillSlot spinUnlockButton;
    [SerializeField] private float hitCooldown = 0.35f;
    [SerializeField] private float maxDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinGravity;

    [Header("Bounce Info")]
    [SerializeField] private UI_SkillSlot bounceUnlockButton;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;
    
    [Header("Peirce Info")]
    [SerializeField] private UI_SkillSlot pierceUnlockButton;
    [SerializeField] private int peirceAmount;
    [SerializeField] private float peirceGravity;

    [Header("Sword Skill Info")]
    [SerializeField] private UI_SkillSlot swordUnlockButton;

    public bool swordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float swordTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Passive Skills")]
    [SerializeField] private UI_SkillSlot timeStopUnlockButton;
    public bool timeStopUnlock { get; private set; }
    [SerializeField] private UI_SkillSlot vulnerableUnlockButton;
    public bool vulnerableUnlock{ get; private set; }
    

    private Vector2 finalDir;

    [Header("Dots Info")] [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        
        GenerateDots();

        SetGravity();
        
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
    }
    
    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }
    
    private void SetGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = peirceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, Player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordSkillScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
            newSwordSkillScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordSkillScript.SetupPierce(peirceAmount);
        else if (swordType == SwordType.Spin)
            newSwordSkillScript.SetupSpin(true, maxDistance, spinDuration, hitCooldown);
        
        
        newSwordSkillScript.SetupSword(finalDir, swordGravity, Player, freezeTimeDuration, swordTimeDuration, returnSpeed);
        
        Player.AssignNewSword(newSword);
        
        DotsActive(false);
    }

    #region Unlock

    void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
            timeStopUnlock = true;
    }
    
    void UnlockVulnerable()
    {
        if (vulnerableUnlockButton.unlocked)
            vulnerableUnlock = true;
    }
    
    void UnlockSword()
    {
        if (swordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }
    
    void UnlockBounceSword()
    {
        if (bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }
    
    void UnlockPierceSword()
    {
        if (pierceUnlockButton.unlocked)
            swordType = SwordType.Pierce;
    }
    
    void UnlockSpinSword()
    {
        if (spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }

    #endregion
    
    #region Aim
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = Player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }
    
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, Player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)Player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}

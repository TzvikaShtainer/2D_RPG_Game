using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Ice And Fire Effect", menuName = "Data/Item Effect/Ice & Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceFirePrefab;
    [SerializeField] private float xVelocity;
    public override void ExecuteEffect(Transform enemyPos)
    {
        Player player = PlayerManager.instance.player;

        bool isThirdAttack = player.PrimaryAttackState.comboCounter == 2;
        Debug.Log(player.GetComponent<Player>().PrimaryAttackState.comboCounter);

        if (isThirdAttack)
        {
            GameObject newIceFireEffect = Instantiate(iceFirePrefab, enemyPos.position, player.transform.rotation);

            newIceFireEffect.GetComponent<Rigidbody2D>().velocity = new Vector2(player.FacingDir * xVelocity, 0);
        
            Destroy(newIceFireEffect, 3f);
        }
    }
}

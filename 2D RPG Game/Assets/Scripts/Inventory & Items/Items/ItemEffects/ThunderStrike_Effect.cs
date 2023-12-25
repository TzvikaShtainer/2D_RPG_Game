using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect", menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderPrefab;
    
    public override void ExecuteEffect(Transform enemyPos)
    {
        GameObject newThunder = Instantiate(thunderPrefab, enemyPos.position, Quaternion.identity);
        
        Destroy(newThunder, 0.3f);
    }
}

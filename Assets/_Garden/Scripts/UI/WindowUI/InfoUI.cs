using UnityEngine;

public class InfoUI : WindowUI
{

    [Header("InfoUI")]
    public InfoDef[] upInfo;
    public InfoDef[] flowerDefs;
    public InfoDef[] enemyDefs;
    public InfoDef[] downInfo;

    public Transform content;

    public InfoIconPrefab infoIconPrefab;

    public void Start()
    {
        SpawnInfo(upInfo);
        SpawnInfo(flowerDefs);
        SpawnInfo(enemyDefs);
        SpawnInfo(downInfo);
    }

    public virtual void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void SpawnInfo(InfoDef[] infoArr)
    {
        for (int i = 0; i < infoArr.Length; i++)
        {
            InfoIconPrefab newIcon = Instantiate(infoIconPrefab, content);
            newIcon.SetUpIcon(infoArr[i]);
        }
    }
}

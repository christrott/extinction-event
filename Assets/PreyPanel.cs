using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyPanel : MonoBehaviour
{
    public GameObject tier1List;
    public GameObject tier2List;
    public GameObject tier3List;

    public void UpdatePreyTier(TileTypes playerTier)
    {
        Debug.Log("UpdatePreyTier(" + playerTier + ")");
        switch(playerTier)
        {
            case TileTypes.PlayerTier1:
                tier1List.SetActive(true);
                tier2List.SetActive(false);
                tier3List.SetActive(false);
                break;
            case TileTypes.PlayerTier2:
                tier1List.SetActive(true);
                tier2List.SetActive(true);
                tier3List.SetActive(false);
                break;
            case TileTypes.PlayerTier3:
                tier1List.SetActive(true);
                tier2List.SetActive(true);
                tier3List.SetActive(true);
                break;
        }
    }
}

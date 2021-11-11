using System.Collections.Generic;
using UnityEngine;

public class EntityPriorityManager : MonoBehaviour
{
    private List<TileTypes> tileTypeRanking;

    void Start()
    {
        tileTypeRanking = new List<TileTypes>();
        tileTypeRanking.Add(TileTypes.EventVoid);
        tileTypeRanking.Add(TileTypes.EventFront);
        tileTypeRanking.Add(TileTypes.PlayerTier3);
        tileTypeRanking.Add(TileTypes.LargeAnimal);
        tileTypeRanking.Add(TileTypes.PlayerTier2);
        tileTypeRanking.Add(TileTypes.MediumAnimal);
        tileTypeRanking.Add(TileTypes.Tree);
        tileTypeRanking.Add(TileTypes.PlayerTier1);
        tileTypeRanking.Add(TileTypes.SmallAnimal);
        tileTypeRanking.Add(TileTypes.Bush);
        tileTypeRanking.Add(TileTypes.Grass);
    }

    public TileTypes GetTileTypePriority(TileTypes activeType, TileTypes subjectType)
    {
        var activeRank = GetTileTypeRankIndex(activeType);
        var subjectRank = GetTileTypeRankIndex(subjectType);

        return activeRank <= subjectRank ? activeType : subjectType;
    }

    private int GetTileTypeRankIndex(TileTypes type)
    {
        return tileTypeRanking.FindIndex((tileType) => tileType == type);
    }
}

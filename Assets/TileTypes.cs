using UnityEngine;

public enum TileTypes
{
    Grass,
    Bush,
    Tree,
    SmallAnimal,
    MediumAnimal,
    LargeAnimal,
    PlayerTier1,
    PlayerTier2,
    PlayerTier3,
    EventFront,
    EventVoid,
    Empty,
    Exit,
};

public struct TileType
{
    public static bool IsPlayerType(TileTypes type)
    {
        return (type == TileTypes.PlayerTier1 || type == TileTypes.PlayerTier2 || type == TileTypes.PlayerTier3);
    }
}

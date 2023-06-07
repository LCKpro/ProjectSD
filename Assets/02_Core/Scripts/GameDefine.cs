public static class GameDefine
{
    /// <summary>
    /// 몬스터 상태 타입
    /// </summary>
    public enum AIStateType
    {
        Idle = 0,
        Attack = 1,
        Damaged = 2,
        Die = 3,
        Chase_CatTower = 4,
        Chase_Attacker = 5,
        Breakable = 6,
        None = 99,
    }

    /// <summary>
    /// 몬스터 공격 타입
    /// </summary>
    public enum AIAttackType
    {
        Melee = 0,
        Range = 1,
        Suicide = 2,
        None = 99,
    }

    public enum BuildingSort
    {
        Apartment = 0,
        ConvenienceStore = 1,
        Restaurant = 2,
        Factory = 3,
        Hospital = 4,
        MilitaryBase = 5,
        GasStation = 6,
        SuperMarket = 7,
        GunStore = 8,
    }

    public enum CraftType
    {
        ATK = 0,
        DFS = 1,
        PD = 2,
        OA = 3,
        None = 99,
    }

    public enum StateType
    {

    }
}

public static class GameDefine
{
    /// 몬스터 상태 타입
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

    /// 몬스터 공격 타입
    public enum AIAttackType
    {
        Melee = 0,
        Range = 1,
        Suicide = 2,
        None = 99,
    }

    /// 파밍 건물 종류
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

    /// 건축 가능한 건물의 타입
    public enum CraftType
    {
        ATK = 0,
        DFS = 1,
        PD = 2,
        OA = 3,
        None = 99,
    }

    /// 게임 상태 타입
    public enum StateType
    {
        Build = 0,  // 건축 중일 때
        Explore = 1,    // 건물 파밍 중일 때
        OpenInventory = 2,  // 인벤토리 열려있을 때
        None = 99,
    }

    /// 아치 상태 타입
    public enum Unit0001StateType
    {
        None = 0,
        Detect = 1,
        MoveToRepair = 2,
        Repair = 3,
    }

    public enum SkillStateType
    {
        Player0001Skill = 1,   // 아치 스킬 발동. 건물 클릭시 

        None = 99,
    }

    // CC(Crowd Control) 타입
    public enum CCType
    {
        Slow = 0,
        Stun = 1,

        None = 99,
    }

    public enum FarmingType
    {
        OnFarming = 0,
        OffFarming = 1,

        None = 99,
    }
}

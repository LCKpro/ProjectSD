public static class GameDefine
{
    /// ���� ���� Ÿ��
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

    /// ���� ���� Ÿ��
    public enum AIAttackType
    {
        Melee = 0,
        Range = 1,
        Suicide = 2,
        None = 99,
    }

    /// �Ĺ� �ǹ� ����
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

    /// ���� ������ �ǹ��� Ÿ��
    public enum CraftType
    {
        ATK = 0,
        DFS = 1,
        PD = 2,
        OA = 3,
        None = 99,
    }

    /// ���� ���� Ÿ��
    public enum StateType
    {
        Build = 0,  // ���� ���� ��
        Explore = 1,    // �ǹ� �Ĺ� ���� ��
        OpenInventory = 2,  // �κ��丮 �������� ��
        None = 99,
    }

    /// ��ġ ���� Ÿ��
    public enum Unit0001StateType
    {
        None = 0,
        Detect = 1,
        MoveToRepair = 2,
        Repair = 3,
    }

    public enum SkillStateType
    {
        Player0001Skill = 1,   // ��ġ ��ų �ߵ�. �ǹ� Ŭ���� 

        None = 99,
    }

    // CC(Crowd Control) Ÿ��
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

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
        None = 99,
    }
}

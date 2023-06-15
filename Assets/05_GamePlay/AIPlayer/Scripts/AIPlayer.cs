
public partial class AIPlayer : Stat
{
    public DieMark dieMark;

    public void SetDieMark()
    {
        dieMark.StartTimer();
    }
}

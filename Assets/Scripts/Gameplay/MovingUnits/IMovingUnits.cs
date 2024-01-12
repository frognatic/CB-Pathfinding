namespace Gameplay.MovingUnits
{
    public interface IMovingUnits
    {
        public void MarkAsLeader();
        public void Unmark();
        public bool IsLeader { get; set; }
    }
}

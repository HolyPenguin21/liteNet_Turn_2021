
public class BrokenLeg : LifetimeBuff
{
    public BrokenLeg()
    {
        this.id = 2;
        this.name = "Broken leg";
        this.description = "Broken leg description ...";
    }

    public override void Effect(Character character)
    {
        character.movement.movePoints_max -= 1;
    }
}

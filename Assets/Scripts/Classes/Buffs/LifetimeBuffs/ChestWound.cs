
public class ChestWound : LifetimeBuff
{
    public ChestWound()
    {
        this.id = 3;
        this.name = "Chest wound";
        this.description = "Chest wound description ...";
    }

    public override void Effect(Character character)
    {
        character.health.hp_max = character.health.hp_max / 2;
        character.health.hp_cur = character.health.hp_cur / 2;
    }
}

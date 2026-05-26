namespace Gardener.GardenerCode.Systems.Nutrient;

public class NutrientData(int value, bool permanent = true)
{
    public int Current { get; private set; } = value;
    public int Initial { get; private set; } = value;
    public bool IsPermanent { get; private set; } = permanent;

    public int Decrease(int amount = 1)
    {
        Current -= amount;
        if (Current < 0)
            Current = 0;

        return Current;
    }
}
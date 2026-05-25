namespace Gardener.GardenerCode.Systems.Nutrient;

public class NutrientData(int value, bool permanent = true)
{
    public int Current { get; private set; } = value;
    public int Initial { get; private set; } = value;
    public bool IsPermanent { get; private set; } = permanent;
}
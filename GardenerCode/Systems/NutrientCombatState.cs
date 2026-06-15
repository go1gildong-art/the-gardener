using System.Runtime.CompilerServices;
namespace Gardener.GardenerCode.Systems;

public static class NutrientCombatState
{
    private static readonly ConditionalWeakTable<object, NutrientCombatData> table = new();

    public class NutrientCombatData
    {
        public int NutrientConsumedThisCombat {get; set;} = 0;
    }

    public static NutrientCombatData Get(object obj)
    {
        return table.GetOrCreateValue(obj);
    }

    public static bool TryGet(object obj, out NutrientCombatData data)
    {
        return table.TryGetValue(obj, out data);
    }

}
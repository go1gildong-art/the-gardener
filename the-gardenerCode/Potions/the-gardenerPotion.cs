using BaseLib.Abstracts;
using BaseLib.Utils;
using the_gardener.the_gardenerCode.Character;

namespace the_gardener.the_gardenerCode.Potions;

[Pool(typeof(the_gardenerPotionPool))]
public abstract class the_gardenerPotion : CustomPotionModel;
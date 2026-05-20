using BaseLib.Abstracts;
using BaseLib.Utils;
using TheGardener.TheGardenerCode.Character;

namespace TheGardener.TheGardenerCode.Potions;

[Pool(typeof(TheGardenerPotionPool))]
public abstract class TheGardenerPotion : CustomPotionModel;
using BaseLib.Abstracts;
using BaseLib.Utils;
using Gardener.GardenerCode.Character;

namespace Gardener.GardenerCode.Potions;

[Pool(typeof(GardenerPotionPool))]
public abstract class GardenerPotion : CustomPotionModel;
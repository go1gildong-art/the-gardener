using MegaCrit.Sts2.Core.Entities.Powers;
using BaseLib.Abstracts;

namespace Gardener.GardenerCode.Powers;

public class FlowerPotPower : CustomPowerModel
{
    	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
}

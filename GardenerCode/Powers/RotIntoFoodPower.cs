
using MegaCrit.Sts2.Core.Entities.Powers;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Commands;

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;


using Gardener.GardenerCode.Extensions;
using Gardener.GardenerCode.Cards;
using Gardener.GardenerCode.Systems;

using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using System.Reflection;

namespace Gardener.GardenerCode.Powers;

public class RotIntoFoodPower : CustomPowerModel, IOnNutrientConsume
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task OnNutrientConsume()
    {
        GD.Print("[DEBOOG] ROTINTOFOOD TRIGGERED");
        Flash();
        await PowerCmd.Apply<RotIntoFoodTempStrengthPower>(null, base.Owner, base.Amount, base.Owner, null);
    }
}
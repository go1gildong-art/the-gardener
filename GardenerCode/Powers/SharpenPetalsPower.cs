
using BaseLib.Extensions;
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
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Gardener.GardenerCode.Powers;

public class SharpenPetalsPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public decimal Thorns { get; set; } = 0;

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    { 
        new PowerVar<ThornsPower>(0)
    };

    public async Task Init(decimal thornsAmount)
    {
        Thorns = thornsAmount;
        DynamicVars["ThornsPower"].BaseValue = thornsAmount;
        await PowerCmd.Apply<ThornsPower>(null, base.Owner, Thorns, base.Owner, null);
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> _, ICombatState __)
    {
        if (side != base.Owner.Side) return;
        if (base.Amount == 1) await BeforeRemoved(this);
        
        await PowerCmd.Decrement(this);
    }

    public async Task BeforeRemoved(PowerModel? power)
    {
        await PowerCmd.Apply<ThornsPower>(null, base.Owner, -Thorns, base.Owner, null);
    } 

    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}
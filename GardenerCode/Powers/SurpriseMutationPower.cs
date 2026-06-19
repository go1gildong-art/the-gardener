
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
using MegaCrit.Sts2.Core.Models;

using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Extensions;

namespace Gardener.GardenerCode.Powers;

public class SurpriseMutationPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    private bool _shouldIgnoreNextInstance;
    protected virtual bool IsPositive => true;
    private int Sign => IsPositive ? 1 : -1;

    private int StrMult => 2;
    private int DexMult => 2;
    private int ThornsMult => 2;
    
    private async Task ApplyEffects(PlayerChoiceContext choiceContext, Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        await PowerCmd.Apply<StrengthPower>(
            choiceContext,
            target,
            (decimal)Sign * amount * StrMult,
            applier,
            cardSource,
            silent: true
        );

        await PowerCmd.Apply<DexterityPower>(
            choiceContext,
            target,
            (decimal)Sign * amount * DexMult,
            applier,
            cardSource,
            silent: true
        );

        await PowerCmd.Apply<ThornsPower>(
            choiceContext,
            target,
            (decimal)Sign * amount * ThornsMult,
            applier,
            cardSource,
            silent: true
        );
    }

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
                await ApplyEffects(new ThrowingPlayerChoiceContext(), target, amount, applier, cardSource);
        }
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!(amount == (decimal)base.Amount) && power == this)
        {
            if (_shouldIgnoreNextInstance)
            {
                _shouldIgnoreNextInstance = false;
            }
            else
            {
                await ApplyEffects(choiceContext, base.Owner, amount, applier, cardSource);
            }
        }
    }
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == base.Owner.Side)
        {
            Flash();
            await PowerCmd.Remove(this);
            await ApplyEffects(null, base.Owner, -base.Amount, base.Owner, null);
        }
    }

    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}
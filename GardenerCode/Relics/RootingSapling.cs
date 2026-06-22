namespace Gardener.GardenerCode.Relics;

using Gardener.GardenerCode.Character;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Entities.Relics;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Entities.Players;

  
[BaseLib.Utils.Pool(typeof(GardenerRelicPool))]
public class RootingSapling() : GardenerRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    private int _baseCount = 3;

    public override int DisplayAmount => Math.Max(0, _baseCount - (base.Owner.PlayerCombatState?.TurnNumber ?? 1) + 1);

    public override bool ShowCounter
    {
        get
        {
            if (CombatManager.Instance.IsInProgress)
            {
                return base.Status == RelicStatus.Normal;
            }
            return false;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] {
        new EnergyVar(1),
        new IntVar("Turns", _baseCount)
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]{
        HoverTipFactory.ForEnergy(this)
    };

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != base.Owner) return amount;
        if (player.PlayerCombatState.TurnNumber <= _baseCount) return amount + base.DynamicVars.Energy.BaseValue;
        return amount;
    }

    public override async Task BeforeCombatStart()
    {
        InvokeDisplayAmountChanged();
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(base.Owner.Creature)) return;
        InvokeDisplayAmountChanged();
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if 
        (
            !participants.Contains(base.Owner.Creature)
            || base.Owner.PlayerCombatState.TurnNumber <= _baseCount
            || base.Status == RelicStatus.Active
        ) return;

        base.Status = RelicStatus.Active;
        InvokeDisplayAmountChanged();
        Flash();
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        base.Status = RelicStatus.Normal;
        InvokeDisplayAmountChanged();
    }
}


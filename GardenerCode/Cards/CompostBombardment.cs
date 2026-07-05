using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models;
using Gardener.GardenerCode.Systems;

[Pool(typeof(GardenerCardPool))]
public class CompostBombardment() : GardenerCard(
  0,
  CardType.Attack,
  CardRarity.Rare,
  TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(20m, DamageProps.card),
        new IntVar("NutrientThreshold", 4),
        new IntVar("CurrentNutrientConsumed", 0)
    };

    protected override bool ShouldGlowGoldInternal => IsPlayable;

    protected override bool IsPlayable => 
    base.DynamicVars["CurrentNutrientConsumed"].BaseValue >= base.DynamicVars["NutrientThreshold"].BaseValue;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).TargetingAllOpponents(base.CombatState)
        .FromCard(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    public override async Task AfterCardEnteredCombat(CardModel card)
	{
		if (card != this || base.IsClone) return;
        UpdateCurrentNutrientConsumed();
	}

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner != base.Owner) return;
        UpdateCurrentNutrientConsumed();
	}

    private void UpdateCurrentNutrientConsumed()
    {
        int amount = NutrientCombatState.Get(base.CombatState ?? this.CombatState).NutrientConsumedThisCombat;
        this.DynamicVars["CurrentNutrientConsumed"].BaseValue = amount;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}

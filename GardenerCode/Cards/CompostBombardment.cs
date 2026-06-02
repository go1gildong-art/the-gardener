using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using MegaCrit.Sts2.Core.Models.CardPools;
using Gardener.GardenerCode.Systems;

[Pool(typeof(GardenerCardPool))]
public class CompostBombardment() : GardenerCode.Cards.GardenerCard(
  0,
  CardType.Attack,
  CardRarity.Rare,
  TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(8m, DamageProps.card),
        new IntVar("NutrientThreshold", 20)
    };

    protected override bool ShouldGlowGoldInternal => IsPlayable;

    protected override bool IsPlayable => 
    NutrientCombatState.Get(base.CombatState).NutrientConsumedThisCombat > base.DynamicVars["NutrientThreshold"].BaseValue;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}

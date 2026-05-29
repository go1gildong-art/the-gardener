using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using MegaCrit.Sts2.Core.Models.CardPools;

[Pool(typeof(GardenerCardPool))]
public class PollenStorm() : GardenerCode.Cards.GardenerCard(
  0,
  CardType.Attack,
  CardRarity.Uncommon,
  TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(11m, DamageProps.card),
        new IntVar("Nutrient", 15),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int cost = base.EnergyCost;
        for (int i = 0; i < cost; i++)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllEnemies()
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            await GardenerCmd.ConsumeNutrient(this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}

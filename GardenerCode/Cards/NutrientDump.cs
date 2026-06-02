using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using System.Security.Cryptography.X509Certificates;
using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

[Pool(typeof(GardenerCardPool))]
public class NutrientDump() : GardenerCode.Cards.GardenerCard(
  2,
  CardType.Attack,
  CardRarity.Common,
  TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(10m, DamageProps.card),
        new IntVar("Nutrient", 6),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        var hand = PileType.Hand.GetPile(base.Owner).Cards;
        decimal nutrientSum = hand.Aggregate(
            0m,
            (acc, card) => acc + card.DynamicVars["Nutrient"]?.BaseValue ?? 0
        );
        decimal totalDamage = (int)(nutrientSum + base.DynamicVars.Damage.BaseValue);

        await DamageCmd.Attack(totalDamage).FromCard(this).Targeting(cardPlay.Target)
        .WithHitFx("vfx/vfx_attack_slash")
        .Execute(choiceContext);

        await GardenerCmd.ConsumeNutrient(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Nutrient"].UpgradeValueBy(2);
    }
}

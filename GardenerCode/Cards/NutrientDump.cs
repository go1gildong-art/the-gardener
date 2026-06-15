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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;

[Pool(typeof(GardenerCardPool))]
public class NutrientDump() : GardenerCode.Cards.GardenerCard(
  2,
  CardType.Attack,
  CardRarity.Common,
  TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", 6),

        new CalculationBaseVar(0m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, Creature? _)
        {
            var hand = PileType.Hand.GetPile(card.Owner).Cards;
            var playArea = PileType.Play.GetPile(card.Owner).Cards;

            Func<decimal, CardModel, decimal> getNutrientSum = (acc, c) => {
                    if (c.DynamicVars.TryGetValue("Nutrient", out var value))
                    {
                        return acc + value.BaseValue;
                    }
                    return acc;
                };

            decimal handNutrientSum = hand.Aggregate(0m, getNutrientSum);
            decimal playNutrientSum = playArea.Aggregate(0m, getNutrientSum);

            return handNutrientSum + playNutrientSum;
        })
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
        .WithHitFx("vfx/vfx_attack_slash")
        .Execute(choiceContext);

        await GardenerCmd.ConsumeNutrient(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(4m);
    }
}

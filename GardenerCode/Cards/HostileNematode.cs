using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;

[Pool(typeof(GardenerCardPool))]
public class HostileNematode() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Skill,
  CardRarity.Uncommon,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", 10),
        new PowerVar<TemporaryThornsPower>(3m),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var hand = base.Owner.Hand;
        if (hand.Count > 0)
        {
            var cardToExhaust = hand.FirstOrDefault();
            if (cardToExhaust != null)
            {
                await CardPileCmd.Move(choiceContext, cardToExhaust, PileType.Exhaust);
            }
        }

        await PowerCmd.Apply<TemporaryThornsPower>(
            choiceContext,
            base.Owner.Creature,
            DynamicVars["TemporaryThornsPower"].BaseValue,
            base.Owner.Creature, this);

        await GardenerCmd.ConsumeNutrient(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["TemporaryThornsPower"].UpgradeValueBy(1);
    }
}

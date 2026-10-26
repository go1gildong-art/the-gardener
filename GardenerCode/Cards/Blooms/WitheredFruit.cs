using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards.Blooms;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

[Pool(typeof(TokenCardPool))]
public class WitheredFruit() : NutrientCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, 4), IBloomCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", Nutrient),
    };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[]
        {
        CardKeyword.Exhaust
        };
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<SlowPower>(
                choiceContext,
                cardPlay.Target,
                1,
                base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
        IncreaseNutrient(2);
    }
}

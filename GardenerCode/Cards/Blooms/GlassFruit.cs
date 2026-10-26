using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards.Blooms;

using Gardener.GardenerCode.Powers;
using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;

  
  
[Pool(typeof(TokenCardPool))]
public class GlassFruit() : NutrientCard(1, CardType.Power, CardRarity.Rare, TargetType.Self, 3), IBloomCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new BlockVar(4m, ValueProp.Move),
        new IntVar("Nutrient", Nutrient)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<GlassFruitPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars.Block.BaseValue,
            base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        IncreaseNutrient(2);
    }
}

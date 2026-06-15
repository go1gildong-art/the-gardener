using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;

[Pool(typeof(GardenerCardPool))]
public class LeafShield : GardenerCard
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public LeafShield() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 20);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
    new BlockVar(8m, BlockProps.card),
        new IntVar("Nutrient", Nutrient),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
                await GardenerCmd.ConsumeNutrient(choiceContext, this);
}

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
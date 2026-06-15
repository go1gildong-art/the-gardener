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
public class BoughShield : GardenerCard
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public BoughShield() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 4);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
    new BlockVar(4m, BlockProps.card),
        new IntVar("Nutrient", Nutrient),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        decimal nutrient = DynamicVars["Nutrient"].BaseValue;

        for (int i = 0; i < nutrient; i++)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, DynamicVars.Block, cardPlay);
        }

        }

    protected override void OnUpgrade()
    {
    }
}

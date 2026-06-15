using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;

using BaseLib.Abstracts;

using MegaCrit.Sts2.Core.Saves.Runs;

[Pool(typeof(GardenerCardPool))]
public class NatureRetreat : GardenerCard
{
    private int _baseNutrient = 12;
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;
    public NatureRetreat() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 12);
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", Nutrient),
        new BlockVar(4m, BlockProps.card),
        new PowerVar<NatureRetreatPower>(1m)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<NatureRetreatPower>(choiceContext, base.Owner.Creature, base.DynamicVars["NatureRetreatPower"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);
        NutrientModifier.GetFrom(this)?.Increase(3);
        // DynamicVars["Nutrient"].UpgradeValueBy(2m);
    }
}

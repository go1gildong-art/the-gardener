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

[Pool(typeof(GardenerCardPool))]
public class Wriggle : GardenerCard
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public Wriggle() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 10);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", Nutrient),
        new CardsVar(2),
        new IntVar("NutrientThreshold", 4),
        new PowerVar<WrigglePower>(4)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        
        if (base.DynamicVars["Nutrient"].BaseValue >= base.DynamicVars["NutrientThreshold"].BaseValue)
        {
            await PowerCmd.Apply<WrigglePower>(
                choiceContext,
                base.Owner.Creature,
                base.DynamicVars["WrigglePower"].BaseValue,
                base.Owner.Creature, this);
        }

        }

    protected override void OnUpgrade()
    {
        NutrientModifier.GetFrom(this)?.Increase(4);
    }
}

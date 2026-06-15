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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.CardSelection;

[Pool(typeof(GardenerCardPool))]
public class HostileNematode : GardenerCard
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public HostileNematode() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 10);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", Nutrient),
        new PowerVar<ThornsPower>(3m),
    };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[] { CardKeyword.Exhaust };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ThornsPower>(
            choiceContext,
            base.Owner.Creature,
            DynamicVars["ThornsPower"].BaseValue,
            base.Owner.Creature, this);

        CardModel cardModel =
        (await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1),
            context: choiceContext,
            player: base.Owner,
            filter: null,
            source: this
            )
        ).FirstOrDefault();
        if (cardModel != null) await CardCmd.Exhaust(choiceContext, cardModel);

        }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

[Pool(typeof(GardenerCardPool))]
public class LifeCocoon() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Skill,
  CardRarity.Rare,
  TargetType.Self), IOnDepleted
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("RelicVar", 2)
    };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[]
    {
        CardKeyword.Exhaust
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
    }

    public async Task OnDepleted()
    {
        AbstractRoom currentRoom = base.CombatState.RunState.CurrentRoom;
        if (currentRoom is CombatRoom combatRoom)
        {
            for (int i = 0; i < DynamicVars["RelicVar"].BaseValue; i++)
            combatRoom.AddExtraReward(
                base.Owner,
                new RelicReward(base.Owner)
                );
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}

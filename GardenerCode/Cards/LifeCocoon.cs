using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Godot;
namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

[Pool(typeof(GardenerCardPool))]
public class LifeCocoon : GardenerCard, IOnDepleted
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public LifeCocoon() : base(
  1,
  CardType.Skill,
  CardRarity.Rare,
  TargetType.Self)
    {
        NutrientModifier.AddTo(this, 6);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("RelicVar", 2),
        new IntVar("Nutrient", Nutrient)
    };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[]
    {
        CardKeyword.Exhaust
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        }

    public async Task OnDepleted(PlayerChoiceContext choiceContext)
    {
        AbstractRoom? currentRoom = base.CombatState?.RunState.CurrentRoom;
        if (currentRoom is CombatRoom combatRoom)
        {
            for (int i = 0; i < DynamicVars["RelicVar"].BaseValue; i++)
            combatRoom.AddExtraReward(
                base.Owner,
                new RelicReward(base.Owner)
                );
        } 
        else
        {
            GD.Print($"[DEBOOG] OnDepleted was triggered but current room is not a CombatRoom. Room: {currentRoom?.GetType().Name}");
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["RelicVar"].UpgradeValueBy(1);
    }
}

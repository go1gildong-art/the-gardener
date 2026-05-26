using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Gardener.GardenerCode.Cards;
using MegaCrit.Sts2.Core.Models;


using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using Godot;

namespace Gardener.GardenerCode.Systems.Nutrient;


public class ConsumeNutrientAction : GameAction
{
    private readonly Player _player;

    private int _turnNumber;
    private readonly PlayerChoiceContext _choiceContext;

    private readonly CardModel _card;
    private readonly int _amount;

    public override ulong OwnerId => _player.NetId;

    public override GameActionType ActionType => GameActionType.CombatPlayPhaseOnly;

    public ConsumeNutrientAction(Player player, CardModel card, PlayerChoiceContext choiceContext, int amount = 1)
    {
        _player = player;
        _card = card;
        _choiceContext = choiceContext;
        _turnNumber = player.PlayerCombatState?.TurnNumber ?? 0;
        _amount = amount;
    }

    protected override async Task ExecuteAction()
    {
        if (_card.DynamicVars["NutrientVar"] == null)
        {
            GD.Print($"[DEBOOG] Card {_card.Id} has no nutrient to consume.");
            return;
        }

        _card.DynamicVars["NutrientVar"].UpgradeValueBy(-_amount);

        if (_card.DynamicVars["NutrientVar"].BaseValue <= 0)
        {
            await new NutrientDepletedAction(_player, _card, _choiceContext).Execute();
        }

        return;
    }

    public override INetAction ToNetAction()
    {
        return new NetConsumeNutrientAction
        {
            turnNumber = _turnNumber,
            card = _card,
            choiceContext = _choiceContext,
            amount = _amount
        };
    }

    public override string ToString()
    {
        return $"{"ConsumeNutrientAction"} for player {_player.NetId} turn {_turnNumber} card {_card.Id}";
    }
}

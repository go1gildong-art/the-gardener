using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Gardener.GardenerCode.Cards;
using MegaCrit.Sts2.Core.Models;
using Godot;

using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;

namespace Gardener.GardenerCode.Systems.Nutrient;

public class NutrientDepletedAction : GameAction
{
	private readonly Player _player;

	private readonly int _turnNumber;

    private readonly CardModel _deplectedCard;

	public override ulong OwnerId => _player.NetId;
	public readonly PlayerChoiceContext _choiceContext;

	public override GameActionType ActionType => GameActionType.CombatPlayPhaseOnly;

	public NutrientDepletedAction(Player player, CardModel deplectedCard, PlayerChoiceContext choiceContext)
	{
		_player = player;
		_deplectedCard = deplectedCard;
		_choiceContext = choiceContext;
		_turnNumber = player.PlayerCombatState?.TurnNumber ?? 0;
	}

	protected override async Task ExecuteAction()
	{
		GD.Print($"Executing NutrientDepletedAction for player {_player.NetId} turn {_turnNumber} card {_deplectedCard.Id}");
		
        await CardPileCmd.RemoveFromCombat(_deplectedCard);
        await CardPileCmd.RemoveFromDeck(_deplectedCard);
	}

	public override INetAction ToNetAction()
	{
		return new NetNutrientDepletedAction
		{
            deplectedCard = _deplectedCard,
			turnNumber = _turnNumber,
			choiceContext = _choiceContext
		};
	}

	public override string ToString()
	{
		return $"{"NutrientDepletedAction"} for player {_player.NetId} turn {_turnNumber} card {_deplectedCard.Id}";
	}
}

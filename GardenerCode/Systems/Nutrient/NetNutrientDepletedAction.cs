

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace Gardener.GardenerCode.Systems.Nutrient;

public struct NetNutrientDepletedAction : INetAction, IPacketSerializable
{
    public int turnNumber;
    public CardModel deplectedCard;
    public PlayerChoiceContext choiceContext;
    public GameAction ToGameAction(Player player)
    {
        return new NutrientDepletedAction(player, deplectedCard, choiceContext);
    }

    public void Serialize(PacketWriter writer)
    {
        writer.WriteInt(turnNumber, 3001);
    }

    public void Deserialize(PacketReader reader)
    {
        turnNumber = reader.ReadInt(3001);
    }

    public override string ToString()
    {
        return $"{"NetNutrientDepletedAction"} turn: {turnNumber}";
    }
}

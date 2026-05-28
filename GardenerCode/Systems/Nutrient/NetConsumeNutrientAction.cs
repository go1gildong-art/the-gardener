

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Models;

namespace Gardener.GardenerCode.Systems.Nutrient;

public struct NetConsumeNutrientAction : INetAction, IPacketSerializable
{
    public int turnNumber;
    public CardModel card;
    public PlayerChoiceContext choiceContext;
    public int amount;
    public GameAction ToGameAction(Player player)
    {
        return new ConsumeNutrientAction(player, card, choiceContext, amount);
    }

    public void Serialize(PacketWriter writer)
    {
        writer.WriteInt(turnNumber, 3002);
    }

    public void Deserialize(PacketReader reader)
    {
        turnNumber = reader.ReadInt(3002);
    }

    public override string ToString()
    {
        return $"{"NetConsumeNutrientAction"} turn: {turnNumber}";
    }
}

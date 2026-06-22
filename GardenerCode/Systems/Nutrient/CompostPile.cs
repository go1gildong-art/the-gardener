using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Gardener.GardenerCode.Systems;
public class CompostPile(PileType pileType) : CustomPile(pileType)
{
    public override bool CardShouldBeVisible(CardModel card)
    {
        throw new NotImplementedException();
    }

    public override Vector2 GetTargetPosition(CardModel card, Vector2 pos)
    {
        throw new NotImplementedException();
    }
}
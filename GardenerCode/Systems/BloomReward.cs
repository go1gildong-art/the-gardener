using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;
using System.Security.Cryptography.X509Certificates;
using MegaCrit.Sts2.Core.Models;
using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Entities.Cards;
using HarmonyLib;
using Gardener.GardenerCode.Cards.Blooms;
using MegaCrit.Sts2.Core.Entities.Players;
using Godot;

namespace Gardener.GardenerCode.Systems;

public class BloomReward
{
    private static readonly Dictionary<RarityOptions, CardRarity> rarityDict = new()
    {
        [RarityOptions.Bud] = CardRarity.Common,
        [RarityOptions.Flower] = CardRarity.Uncommon,
        [RarityOptions.Fruit] = CardRarity.Rare
    };

    public static CardReward Generate(RarityOptions rarityOption, Player player, int amount = 3)
    {
        TokenCardPool tokenCardPool = ModelDb.CardPool<TokenCardPool>();

        CardCreationOptions option = CardCreationOptions.ForNonCombatWithUniformOdds
        (
            [tokenCardPool],
            c =>
            {
                // Bloom card check
                if (c is not IBloomCard) return false;

                // Rarity check
                CardRarity[] rarityArray = [];
                foreach (var (flag, rarity) in rarityDict)
                {
                    if ((rarityOption & flag) == 0 && c.Rarity == rarity) return false;
                }

                return true;
            }
        );

        GD.Print($"[DEBOOG] BloomReward.Generate: Generated {amount} card reward(s) with rarity option {rarityOption}.");
        return new CardReward(option, amount, player);
    }
}

public enum RarityOptions
{
    Bud = 1 << 0, // 001
    Flower = 1 << 1, // 010
    Fruit = 1 << 2  // 100
}
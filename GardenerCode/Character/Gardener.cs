using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Gardener.GardenerCode.Extensions;
using Gardener.GardenerCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using Gardener.GardenerCode.Cards;
using MegaCrit.Sts2.Core.Helpers;

namespace Gardener.GardenerCode.Character;


public class Gardener : PlaceholderCharacterModel
{
    public const string CharacterId = "Gardener";

    public static readonly Color Color = new("89e73c");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeGardener>(),
        ModelDb.Card<StrikeGardener>(),
        ModelDb.Card<StrikeGardener>(),
        ModelDb.Card<StrikeGardener>(),
        ModelDb.Card<DefendGardener>(),
        ModelDb.Card<DefendGardener>(),
        ModelDb.Card<DefendGardener>(),
        ModelDb.Card<DefendGardener>(),
        ModelDb.Card<Weeding>(),
        ModelDb.Card<NatureRetreat>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<TwinklingSprout>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<GardenerCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<GardenerRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<GardenerPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets. 
        These are just some of the simplest assets, given some placeholders to differentiate your character with. 
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    public override string CustomVisualPath => "res://Gardener/scenes/gardener.tscn";
    // public override string CustomTrailPath => SceneHelper.GetScenePath("vfx/card_trail_" + PlaceholderID);
    // public override string? CustomMapMarkerPath => ImageHelper.GetImagePath("packed/map/icons/map_marker_" + PlaceholderID + ".png");
    // public override string CustomIconPath => SceneHelper.GetScenePath("ui/character_icons/" + PlaceholderID + "_icon");
    // public override string? CustomIconTexturePath => ImageHelper.GetImagePath("ui/top_panel/character_icon_" + PlaceholderID + ".png");
    // public override string CustomEnergyCounterPath => SceneHelper.GetScenePath("combat/energy_counters/" + PlaceholderID + "_energy_counter");
    // public override string CustomRestSiteAnimPath => SceneHelper.GetScenePath("rest_site/characters/" + PlaceholderID + "_rest_site");
    // public override string CustomMerchantAnimPath => SceneHelper.GetScenePath("merchant/characters/" + PlaceholderID + "_merchant");
    // public override string CustomArmPointingTexturePath => ImageHelper.GetImagePath("ui / hands / " + PlaceholderID + "_arm_point.png");
    // public override string CustomArmRockTexturePath => ImageHelper.GetImagePath("ui / hands / " + PlaceholderID + "_arm_rock.png");
    // public override string CustomArmPaperTexturePath => ImageHelper.GetImagePath("ui/hands/" + PlaceholderID + "_arm_paper.png");
    // public override string CustomArmScissorsTexturePath => ImageHelper.GetImagePath("ui/hands/" + PlaceholderID + "_arm_scissors.png");
    // public override string CustomCharacterSelectBg => SceneHelper.GetScenePath("screens / char_select / char_select_bg_" + PlaceholderID);
    // public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/" + PlaceholderID + "_transition_mat.tres";
    // public override string? CustomCharacterSelectIconPath => ImageHelper.GetImagePath("packed/character_select/char_select_" + PlaceholderID + ".png");
    // public override string? CustomCharacterSelectLockedIconPath => ImageHelper.GetImagePath("packed/character_select/char_select_" + PlaceholderID + "_locked.png");
}
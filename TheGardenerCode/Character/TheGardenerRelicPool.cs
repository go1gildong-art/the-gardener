using BaseLib.Abstracts;
using TheGardener.TheGardenerCode.Extensions;
using Godot;

namespace TheGardener.TheGardenerCode.Character;

public class TheGardenerRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheGardener.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
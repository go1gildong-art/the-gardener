using BaseLib.Abstracts;
using Gardener.GardenerCode.Extensions;
using Godot;

namespace Gardener.GardenerCode.Character;

public class GardenerRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Gardener.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
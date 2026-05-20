using BaseLib.Abstracts;
using the_gardener.the_gardenerCode.Extensions;
using Godot;

namespace the_gardener.the_gardenerCode.Character;

public class the_gardenerRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => the_gardener.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
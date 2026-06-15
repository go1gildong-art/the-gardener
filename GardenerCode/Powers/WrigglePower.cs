using BaseLib.Extensions;
using Gardener.GardenerCode.Powers;
using Gardener.GardenerCode.Extensions;
namespace Gardener.GardenerCode.Powers;

  
public class WrigglePower : TemporaryThornsPower {
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}
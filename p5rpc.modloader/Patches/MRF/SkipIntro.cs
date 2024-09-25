using p5rpc.modloader.Patches.Common;
using Reloaded.Memory.Sources;

namespace p5rpc.modloader.Patches.MRF;

/// <summary>
/// Skips the game introduction sequence.
/// </summary>
internal class SkipIntro
{
    public static void Activate(in PatchContext context)
    {
        var baseAddr = context.BaseAddress;
        if (!context.Config.MRFConfig.IntroSkip)
            return;

        context.ScanHelper.FindPatternOffset("48 89 5C 24 ?? 55 56 57 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC D0 08 00 00", (offset) =>
            Memory.Instance.SafeWriteRaw((nuint)(baseAddr + offset), new byte[] { 0xb8, 0x01, 0x00, 0x00, 0x00, 0xc3 }),
            "Introduction Skip");
    }
}
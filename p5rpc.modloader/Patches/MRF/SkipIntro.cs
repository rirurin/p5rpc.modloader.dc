using p5rpc.modloader.Patches.Common;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Sources;
using System.Runtime.InteropServices;

namespace p5rpc.modloader.Patches.MRF;

/// <summary>
/// Skips the game introduction sequence.
/// </summary>
internal class SkipIntro
{
    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    public struct TitleTask
    {
        [FieldOffset(0x8)] public int State;
    }

    private static IHook<TitleTaskLoopInner> _titleTaskLoopInner = null!;
    private static FileEmulationFramework.Lib.Utilities.Logger _logger = null!;

    [Function(CallingConventions.Microsoft)]
    public struct TitleTaskLoopInner { public FuncPtr<BlittablePointer<TitleTask>, byte> Value; }
    [UnmanagedCallersOnly]
    public unsafe static byte TitleTaskLoopInnerImpl(TitleTask* title)
    {
        if (title->State == 39)
            title->State = 63;
        return _titleTaskLoopInner.OriginalFunction.Value.Invoke(title);
    }

    public static void Activate(in PatchContext context)
    {
        var baseAddr = context.BaseAddress;
        if (!context.Config.MRFConfig.IntroSkip)
            return;
        IReloadedHooks Hook = context.Hooks;
        _logger = context.Logger;
        context.ScanHelper.FindPatternOffset("48 89 5C 24 ?? 55 56 57 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC D0 08 00 00", (offset) =>
            _titleTaskLoopInner = Hook.CreateHook<TitleTaskLoopInner>(typeof(SkipIntro), nameof(TitleTaskLoopInnerImpl), baseAddr + offset).Activate(),
            "Introduction Skip");
    }
}
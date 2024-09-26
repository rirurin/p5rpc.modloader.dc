using p5rpc.modloader.Patches.Common;
using Reloaded.Memory.Sources;

namespace p5rpc.modloader.Patches.MRF
{
    internal class Force4kAssets
    {
        public static void Activate(in PatchContext context)
        {
            var baseAddr = context.BaseAddress;
            if (!context.Config.MRFConfig.Force4k)
                return;

            context.ScanHelper.FindPatternOffset("83 FB 01 0F 44 C1", (offset) =>
                Memory.Instance.SafeWriteRaw((nuint)(baseAddr + offset), new byte[] { 0xc7, 0xc0, 0x02, 0x00, 0x00, 0x00 }), // MOV EAX, 2
            "Force 4k Assets");
        }
    }
}

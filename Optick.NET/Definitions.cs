using System;
using System.Runtime.InteropServices;

namespace Optick.NET
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VulkanFunctions
    {
        public nint vkGetPhysicalDeviceProperties;
        public nint vkCreateQueryPool;
        public nint vkCreateCommandPool;
        public nint vkAllocateCommandBuffers;
        public nint vkCreateFence;
        public nint vkCmdResetQueryPool;
        public nint vkQueueSubmit;
        public nint vkWaitForFences;
        public nint vkResetCommandBuffer;
        public nint vkCmdWriteTimestamp;
        public nint vkGetQueryPoolResults;
        public nint vkBeginCommandBuffer;
        public nint vkEndCommandBuffer;
        public nint vkResetFences;
        public nint vkDestroyCommandPool;
        public nint vkDestroyQueryPool;
        public nint vkDestroyFence;
        public nint vkFreeCommandBuffers;
    }

    // System.Windows.Media.Colors is not a .NET Core API
    public enum Color : uint
    {
        Null = 0x00000000,
        AliceBlue = 0xFFF0F8FF,
        AntiqueWhite = 0xFFFAEBD7,
        Aqua = 0xFF00FFFF,
        Aquamarine = 0xFF7FFFD4,
        Azure = 0xFFF0FFFF,
        Beige = 0xFFF5F5DC,
        Bisque = 0xFFFFE4C4,
        Black = 0xFF000000,
        BlanchedAlmond = 0xFFFFEBCD,
        Blue = 0xFF0000FF,
        BlueViolet = 0xFF8A2BE2,
        Brown = 0xFFA52A2A,
        BurlyWood = 0xFFDEB887,
        CadetBlue = 0xFF5F9EA0,
        Chartreuse = 0xFF7FFF00,
        Chocolate = 0xFFD2691E,
        Coral = 0xFFFF7F50,
        CornflowerBlue = 0xFF6495ED,
        Cornsilk = 0xFFFFF8DC,
        Crimson = 0xFFDC143C,
        Cyan = 0xFF00FFFF,
        DarkBlue = 0xFF00008B,
        DarkCyan = 0xFF008B8B,
        DarkGoldenRod = 0xFFB8860B,
        DarkGray = 0xFFA9A9A9,
        DarkGreen = 0xFF006400,
        DarkKhaki = 0xFFBDB76B,
        DarkMagenta = 0xFF8B008B,
        DarkOliveGreen = 0xFF556B2F,
        DarkOrange = 0xFFFF8C00,
        DarkOrchid = 0xFF9932CC,
        DarkRed = 0xFF8B0000,
        DarkSalmon = 0xFFE9967A,
        DarkSeaGreen = 0xFF8FBC8F,
        DarkSlateBlue = 0xFF483D8B,
        DarkSlateGray = 0xFF2F4F4F,
        DarkTurquoise = 0xFF00CED1,
        DarkViolet = 0xFF9400D3,
        DeepPink = 0xFFFF1493,
        DeepSkyBlue = 0xFF00BFFF,
        DimGray = 0xFF696969,
        DodgerBlue = 0xFF1E90FF,
        FireBrick = 0xFFB22222,
        FloralWhite = 0xFFFFFAF0,
        ForestGreen = 0xFF228B22,
        Fuchsia = 0xFFFF00FF,
        Gainsboro = 0xFFDCDCDC,
        GhostWhite = 0xFFF8F8FF,
        Gold = 0xFFFFD700,
        GoldenRod = 0xFFDAA520,
        Gray = 0xFF808080,
        Green = 0xFF008000,
        GreenYellow = 0xFFADFF2F,
        HoneyDew = 0xFFF0FFF0,
        HotPink = 0xFFFF69B4,
        IndianRed = 0xFFCD5C5C,
        Indigo = 0xFF4B0082,
        Ivory = 0xFFFFFFF0,
        Khaki = 0xFFF0E68C,
        Lavender = 0xFFE6E6FA,
        LavenderBlush = 0xFFFFF0F5,
        LawnGreen = 0xFF7CFC00,
        LemonChiffon = 0xFFFFFACD,
        LightBlue = 0xFFADD8E6,
        LightCoral = 0xFFF08080,
        LightCyan = 0xFFE0FFFF,
        LightGoldenRodYellow = 0xFFFAFAD2,
        LightGray = 0xFFD3D3D3,
        LightGreen = 0xFF90EE90,
        LightPink = 0xFFFFB6C1,
        LightSalmon = 0xFFFFA07A,
        LightSeaGreen = 0xFF20B2AA,
        LightSkyBlue = 0xFF87CEFA,
        LightSlateGray = 0xFF778899,
        LightSteelBlue = 0xFFB0C4DE,
        LightYellow = 0xFFFFFFE0,
        Lime = 0xFF00FF00,
        LimeGreen = 0xFF32CD32,
        Linen = 0xFFFAF0E6,
        Magenta = 0xFFFF00FF,
        Maroon = 0xFF800000,
        MediumAquaMarine = 0xFF66CDAA,
        MediumBlue = 0xFF0000CD,
        MediumOrchid = 0xFFBA55D3,
        MediumPurple = 0xFF9370DB,
        MediumSeaGreen = 0xFF3CB371,
        MediumSlateBlue = 0xFF7B68EE,
        MediumSpringGreen = 0xFF00FA9A,
        MediumTurquoise = 0xFF48D1CC,
        MediumVioletRed = 0xFFC71585,
        MidnightBlue = 0xFF191970,
        MintCream = 0xFFF5FFFA,
        MistyRose = 0xFFFFE4E1,
        Moccasin = 0xFFFFE4B5,
        NavajoWhite = 0xFFFFDEAD,
        Navy = 0xFF000080,
        OldLace = 0xFFFDF5E6,
        Olive = 0xFF808000,
        OliveDrab = 0xFF6B8E23,
        Orange = 0xFFFFA500,
        OrangeRed = 0xFFFF4500,
        Orchid = 0xFFDA70D6,
        PaleGoldenRod = 0xFFEEE8AA,
        PaleGreen = 0xFF98FB98,
        PaleTurquoise = 0xFFAFEEEE,
        PaleVioletRed = 0xFFDB7093,
        PapayaWhip = 0xFFFFEFD5,
        PeachPuff = 0xFFFFDAB9,
        Peru = 0xFFCD853F,
        Pink = 0xFFFFC0CB,
        Plum = 0xFFDDA0DD,
        PowderBlue = 0xFFB0E0E6,
        Purple = 0xFF800080,
        Red = 0xFFFF0000,
        RosyBrown = 0xFFBC8F8F,
        RoyalBlue = 0xFF4169E1,
        SaddleBrown = 0xFF8B4513,
        Salmon = 0xFFFA8072,
        SandyBrown = 0xFFF4A460,
        SeaGreen = 0xFF2E8B57,
        SeaShell = 0xFFFFF5EE,
        Sienna = 0xFFA0522D,
        Silver = 0xFFC0C0C0,
        SkyBlue = 0xFF87CEEB,
        SlateBlue = 0xFF6A5ACD,
        SlateGray = 0xFF708090,
        Snow = 0xFFFFFAFA,
        SpringGreen = 0xFF00FF7F,
        SteelBlue = 0xFF4682B4,
        Tan = 0xFFD2B48C,
        Teal = 0xFF008080,
        Thistle = 0xFFD8BFD8,
        Tomato = 0xFFFF6347,
        Turquoise = 0xFF40E0D0,
        Violet = 0xFFEE82EE,
        Wheat = 0xFFF5DEB3,
        White = 0xFFFFFFFF,
        WhiteSmoke = 0xFFF5F5F5,
        Yellow = 0xFFFFFF00,
        YellowGreen = 0xFF9ACD32,
    }

    public enum Filter : uint
    {
        None,

        // CPU
        AI,
        Animation,
        Audio,
        Debug,
        Camera,
        Cloth,
        GameLogic,
        Input,
        Navigation,
        Network,
        Physics,
        Rendering,
        Scene,
        Script,
        Streaming,
        UI,
        VFX,
        Visibility,
        Wait,

        // IO
        IO,

        // GPU
        GPU_Cloth,
        GPU_Lighting,
        GPU_PostFX,
        GPU_Reflections,
        GPU_Scene,
        GPU_Shadows,
        GPU_UI,
        GPU_VFX,
        GPU_Water,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Category
    {
        public Category(ulong value)
        {
            Value = value;
        }

        public ulong Value { get; }
        public uint CategoryMask => (uint)(Value >> 32);
        public uint CategoryColor => (uint)(Value & (ulong)(Math.Pow(2, 32) - 1));

        // CPU
        public static Category None => Optick.MakeCategory(Filter.None, Color.Null);
        public static Category AI => Optick.MakeCategory(Filter.AI, Color.Purple);
        public static Category Animation => Optick.MakeCategory(Filter.Animation, Color.LightSkyBlue);
        public static Category Audio => Optick.MakeCategory(Filter.Audio, Color.HotPink);
        public static Category Debug => Optick.MakeCategory(Filter.Debug, Color.Black);
        public static Category Camera => Optick.MakeCategory(Filter.Camera, Color.Black);
        public static Category Cloth => Optick.MakeCategory(Filter.Cloth, Color.DarkGreen);
        public static Category GameLogic => Optick.MakeCategory(Filter.GameLogic, Color.RoyalBlue);
        public static Category Input => Optick.MakeCategory(Filter.Input, Color.Ivory);
        public static Category Navigation => Optick.MakeCategory(Filter.Navigation, Color.Magenta);
        public static Category Network => Optick.MakeCategory(Filter.Network, Color.Olive);
        public static Category Physics => Optick.MakeCategory(Filter.Physics, Color.LawnGreen);
        public static Category Rendering => Optick.MakeCategory(Filter.Rendering, Color.BurlyWood);
        public static Category Scene => Optick.MakeCategory(Filter.Scene, Color.RoyalBlue);
        public static Category Script => Optick.MakeCategory(Filter.Script, Color.Plum);
        public static Category Streaming => Optick.MakeCategory(Filter.Streaming, Color.Gold);
        public static Category UI => Optick.MakeCategory(Filter.UI, Color.PaleTurquoise);
        public static Category VFX => Optick.MakeCategory(Filter.VFX, Color.SaddleBrown);
        public static Category Visibility => Optick.MakeCategory(Filter.Visibility, Color.Snow);
        public static Category Wait => Optick.MakeCategory(Filter.Wait, Color.Tomato);
        public static Category WaitEmpty => Optick.MakeCategory(Filter.Wait, Color.White);

        // IO
        public static Category IO => Optick.MakeCategory(Filter.IO, Color.Khaki);

        // GPU
        public static Category GPU_Cloth => Optick.MakeCategory(Filter.GPU_Cloth, Color.DarkGreen);
        public static Category GPU_Lighting => Optick.MakeCategory(Filter.GPU_Lighting, Color.Khaki);
        public static Category GPU_PostFX => Optick.MakeCategory(Filter.GPU_PostFX, Color.Maroon);
        public static Category GPU_Reflections => Optick.MakeCategory(Filter.GPU_Reflections, Color.CadetBlue);
        public static Category GPU_Scene => Optick.MakeCategory(Filter.GPU_Scene, Color.RoyalBlue);
        public static Category GPU_Shadows => Optick.MakeCategory(Filter.GPU_Shadows, Color.LightSlateGray);
        public static Category GPU_UI => Optick.MakeCategory(Filter.GPU_UI, Color.PaleTurquoise);
        public static Category GPU_VFX => Optick.MakeCategory(Filter.GPU_VFX, Color.SaddleBrown);
        public static Category GPU_Water => Optick.MakeCategory(Filter.GPU_Water, Color.SteelBlue);
    }

    [Flags]
    public enum Mode : int
    {
        // OFF
        OFF = 0x0,
        // Collect Categories (top-level events)
        INSTRUMENTATION_CATEGORIES = (1 << 0),
        // Collect Events
        INSTRUMENTATION_EVENTS = (1 << 1),
        // Collect Events + Categories
        INSTRUMENTATION = (INSTRUMENTATION_CATEGORIES | INSTRUMENTATION_EVENTS),
        // Legacy (keep for compatibility reasons)
        SAMPLING = (1 << 2),
        // Collect Data Tags
        TAGS = (1 << 3),
        // Enable Autosampling Events (automatic callstacks)
        AUTOSAMPLING = (1 << 4),
        // Enable Switch-Contexts Events
        SWITCH_CONTEXT = (1 << 5),
        // Collect I/O Events
        IO = (1 << 6),
        // Collect GPU Events
        GPU = (1 << 7),
        END_SCREENSHOT = (1 << 8),
        RESERVED_0 = (1 << 9),
        RESERVED_1 = (1 << 10),
        // Collect HW Events
        HW_COUNTERS = (1 << 11),
        // Collect Events in Live mode
        LIVE = (1 << 12),
        RESERVED_2 = (1 << 13),
        RESERVED_3 = (1 << 14),
        RESERVED_4 = (1 << 15),
        // Collect System Calls
        SYS_CALLS = (1 << 16),
        // Collect Events from Other Processes
        OTHER_PROCESSES = (1 << 17),
        // Automation
        NOGUI = (1 << 18),

        TRACER = AUTOSAMPLING | SWITCH_CONTEXT | SYS_CALLS,
        DEFAULT = INSTRUMENTATION | TAGS | AUTOSAMPLING | SWITCH_CONTEXT | IO | GPU | SYS_CALLS | OTHER_PROCESSES,
    }

    public enum FrameType
    {
        CPU,
        GPU,
        Render,
        COUNT,

        NONE = -1
    }

    [Flags]
    public enum ThreadMask : int
    {
        None = 0,
        Main = 1 << 0,
        GPU = 1 << 1,
        IO = 1 << 2,
        Idle = 1 << 3,
        Render = 1 << 4
    }

    public enum State : int
    {
        // Starting a new capture
        START_CAPTURE,

        // Stopping current capture
        STOP_CAPTURE,

        // Dumping capture to the GUI
        // Useful for attaching summary and screenshot to the capture
        DUMP_CAPTURE,

        // Cancel current capture
        CANCEL_CAPTURE,
    }

    [UnmanagedFunctionPointer(Optick.Convention)]
    public delegate bool StateCallback(State state);

    public enum FileType : int
    {
        OPTICK_IMAGE,
        OPTICK_TEXT,
        OPTICK_OTHER
    }

    
}
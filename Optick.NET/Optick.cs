namespace Optick.NET
{
    public static partial class Optick
    {
        public static Category MakeCategory(Filter filter, Color color) => new Category((((ulong)(1)) << ((int)filter + 32)) | (ulong)color);

        // TODO: binding shenanigans
    }
}
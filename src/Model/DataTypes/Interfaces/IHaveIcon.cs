namespace DewIt.Model.DataTypes
{
    public interface IHaveIcon
    {
        // Icon: can't use system.drawing on mac/iOS/Android
        // read https://github.com/dotnet/Microsoft.Maui.Graphics
        // or try https://github.com/mono/SkiaSharp
    }
}
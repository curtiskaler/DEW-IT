using DewIt.Model.DataTypes;

namespace DewIt.Model.Tests;

internal static class URNTestHelper
{
    internal static string ResolveString(string NID, string NSS, string? RComponent, string? QComponent,
        string? FComponent)
    {
        var urnString = $"{URN.URNScheme}:{NID}:{NSS}";
        if (!string.IsNullOrEmpty(RComponent)) urnString += $"?+{RComponent}";
        if (!string.IsNullOrEmpty(QComponent)) urnString += $"?={QComponent}";
        if (!string.IsNullOrEmpty(FComponent)) urnString += $"#{FComponent}";
        return urnString;
    }
}
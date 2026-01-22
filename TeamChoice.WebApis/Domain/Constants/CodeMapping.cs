using System;
using System.Collections.Generic;

namespace TeamChoice.WebApis.Domain.Constants;

public static class CodeMapping
{
    private static readonly IReadOnlyDictionary<string, string> Mappings =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["00010"] = "SOS0000001",
            ["00003"] = "SOS0000002",
            ["00006"] = "SOK0000001"
        };

    public static string GetMappedValueByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code must not be null or empty.", nameof(code));
        }

        if (!Mappings.TryGetValue(code, out var mappedValue))
        {
            throw new ArgumentException($"Invalid code: {code}", nameof(code));
        }

        return mappedValue;
    }
}

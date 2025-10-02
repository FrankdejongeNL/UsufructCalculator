namespace UsufructCalculator.Api.Constants;

/// <summary>
/// API version constants for route configuration.
/// </summary>
/// <remarks>
/// When adding a new version:
/// 1. Add a new constant (e.g., V2 = "v2")
/// 2. Create a new controller or use conditional logic in existing controller
/// 3. Update Swagger configuration to include the new version
/// 4. Consider deprecating old versions with [Obsolete] attribute.
/// </remarks>
public static class ApiVersions
{
    /// <summary>
    /// API version 1.0 - Initial release.
    /// </summary>
    public const string V1 = "v1";
}

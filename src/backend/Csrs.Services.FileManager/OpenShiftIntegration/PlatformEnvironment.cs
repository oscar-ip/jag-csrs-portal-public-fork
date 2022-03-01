namespace Csrs.Services.FileManager.OpenShiftIntegration
{
    public static class PlatformEnvironment
    {
        public static bool IsOpenShift => OpenShiftEnvironment.IsOpenShift;
    }
}

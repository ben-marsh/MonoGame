using System;

namespace MonoGame.Tools.Pipeline
{
    public static class Global
    {
        public static bool Unix
            => !OperatingSystem.IsWindows();
    }
}

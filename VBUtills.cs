namespace VBUtills
{
    public static class VBUtills
    {
        private static bool _initialized;
        
        public static void Initialize()
        {
            if (_initialized) return;
            new Harmony("VitByr.VBUtills").PatchAll(Assembly.GetExecutingAssembly());
            _initialized = true;
        }
    }
}
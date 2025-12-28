namespace VBUtills
{
    public static class VBUtills
    {
        private static bool _initialized;
        
        public static void Initialize()
        {
            RegisterPrefab.VB_AddCreatureWithConfig("name_bundle", 
                ("Mob1_prefab", new CreatureConfig()), // если дроп и спавн не нужен
                ("Mob2_prefab", new CreatureConfig
                {
                    DropConfigs = new DropConfig[]
                    {
                        new DropConfig { Item = "JuteBlue", MinAmount = 1, MaxAmount = 3, Chance = 100, OnePerPlayer = false, LevelMultiplier = true },
                        new DropConfig { Item = "TrophyCultist", MinAmount = 1, MaxAmount = 1, Chance = 10, OnePerPlayer = false, LevelMultiplier = false }
                    }
                }), // если нужен только дроп
                ("Mob3_prefab", new CreatureConfig
                {
                    SpawnConfigs = new SpawnConfig[]
                    {
                        new SpawnConfig
                        {
                            WorldSpawnEnabled = true, Biome = Heightmap.Biome.DeepNorth,
                            BiomeArea = Heightmap.BiomeArea.Median, MaxSpawned = 1, SpawnInterval = 800, SpawnChance = 20, MinLevel = 1, MaxLevel = 3, MinGroupSize = 1,
                            MaxGroupSize = 2, GroupRadius = 10, SpawnAtDay = true, SpawnAtNight = true, MinAltitude = 2f, SpawnInForest = true, SpawnOutsideForest = true
                        }
                    }
                }), // если нужен только спавн
                ("Mob4_prefab", new CreatureConfig
                {
                    DropConfigs = new DropConfig[]
                    {
                        new DropConfig { Item = "JuteBlue", MinAmount = 1, MaxAmount = 3, Chance = 100, OnePerPlayer = false, LevelMultiplier = true },
                        new DropConfig { Item = "TrophyCultist", MinAmount = 1, MaxAmount = 1, Chance = 10, OnePerPlayer = false, LevelMultiplier = false }
                    },
                    SpawnConfigs = new SpawnConfig[]
                    {
                        new SpawnConfig
                        {
                            WorldSpawnEnabled = true, Biome = Heightmap.Biome.Meadows,
                            BiomeArea = Heightmap.BiomeArea.Median, MaxSpawned = 1, SpawnInterval = 800, SpawnChance = 20, MinLevel = 1, MaxLevel = 3, MinGroupSize = 1,
                            MaxGroupSize = 2, GroupRadius = 10, SpawnAtDay = true, SpawnAtNight = true, MinAltitude = 2f, SpawnInForest = true, SpawnOutsideForest = true
                        },
                        new SpawnConfig
                        {
                        WorldSpawnEnabled = true, Biome = Heightmap.Biome.DeepNorth,
                        BiomeArea = Heightmap.BiomeArea.Median, MaxSpawned = 1, SpawnInterval = 1800, SpawnChance = 50, MinLevel = 1, MaxLevel = 3, MinGroupSize = 10,
                        MaxGroupSize = 20, GroupRadius = 10, SpawnAtDay = true, SpawnAtNight = true, MinAltitude = 2f, SpawnInForest = true, SpawnOutsideForest = true
                        }
                    }
                }) // если нужен дроп и спавн с разными настройками
            );
            
            if (_initialized) return;
            new Harmony("VitByr.VBUtills").PatchAll(Assembly.GetExecutingAssembly());
            _initialized = true;
        }
    }
}
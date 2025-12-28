Данный набор утилит сделан для использования с библиотекой Jotunn. Я её сделал для более удобной работы и чтобы избавиться от ненужной объемности в коде - вместо условных 10-ти строк получается одна.
В него входят такие функции:
* Регистрация префаба (VB_AddPrefab)
* Регистрация предмета (VB_AddItem)
* Регистрация предмета с рецептом (VB_AddItemRecipe)
* Регистрация моба с дропом и спавном (VB_AddCreatureWithConfig)
* Регистрация SE эффекта (VB_AddSE)
* Регистрация постройки (VB_AddPiece)
* Копирование части компонента с одной цели на другую (Replace)
* Прямая запись эффекта в цель (AddEffect)
* Копирование Animator с одной цели на другую (ReplaceAnimator)
* Копирование эффектов шагов с одной цели на другую (ReplaceFoot)
* Исправление шейдеров (встроен во все регистрации и отдельно использовать нет необходимости)

Примеры использования:
Пишется в Init()/ Awake(). В начале указывается ваш bundle файл.Через запятую прописываем необходимые префабы для регистрации.
- RegisterPrefab.VB_AddPrefab("name_bundle", "sfx_build_hammer_wood_d", "sfx_hardwood_destroyed_d", "vfx_Place_wood", "vfx_Saw_Dust_d");
Нужен для эффектов и статичных объектов.

- RegisterPrefab.VB_AddItem("name_bundle", "Meat", "RawMeat");
Нужен для предметов, которым не нужен рецепт или для атак мобов.

- RegisterPrefab.VB_AddItemRecipe("name_bundle", 
                ("dual_axe_bronze", 1, new[] { ("Bronze", 20, 10, true), ("RoundLog", 2, 0, true) }, CraftingStations.Forge, 1),
                ("dual_axe_iron", 1, new[] { ("Iron", 20, 10, true), ("RoundLog", 2, 0, true) }, CraftingStations.Forge, 2)
            );
Нужен для регистрации предметов, которые необходимо добавить в крафт.

- RegisterPrefab.VB_AddPiece(
                "name_bundle", PieceTables.Hammer, PieceCategories.Building, CraftingStations.Workbench,
                ("roof", new[] { ("Resin", 2, true), ("Wood", 2, true) }),
                ("roof_45", new[] { ("Resin", 2, true), ("Wood", 2, true) })
            );
Нужен для регистрации построек.

- RegisterPrefab.VB_AddCreatureWithConfig("name_bundle", 
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
Нужен для регистрации мобов.

- RegisterPrefab.VB_AddSE(name_bundle", "SE_Debuff_1", "SE_Debuff_2");
Нужен для регистрации SE эффекта, который создан не на базе ванильного.

Следующие патчи используются в: 
[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
        [HarmonyPostfix]
        public static void Patch()
        {
            componet();
        }
Если клонируете всё и вам нет дела до размера bundle файла, то дальше можно не смотреть.

- ComponentPatch.ReplaceAnimator("Ulv_d", "Ulv");
Нужен для взятиия анимаций от донора.

- ComponentPatch.Replace<Humanoid>("Ulv_d", "Ulv", (c, v) =>
            {
                c.m_hitEffects.m_effectPrefabs = v.m_hitEffects.m_effectPrefabs;
                c.m_critHitEffects.m_effectPrefabs = v.m_critHitEffects.m_effectPrefabs;
                c.m_backstabHitEffects.m_effectPrefabs = v.m_backstabHitEffects.m_effectPrefabs;
                c.m_waterEffects.m_effectPrefabs = v.m_waterEffects.m_effectPrefabs;
            });
Нужен для взятия от донора нужных нам эффектов.

- ComponentPatch.AddEffect<Humanoid>("Ulv_d", "sfx_ulv_death", h => h.m_deathEffects);
Нужен для указания конкретного эффекта - если нам не нужен весь список от донора.

- ComponentPatch.ReplaceFoot("VB_Deep_Cultist", "Fenring_Cultist");
Нужен для взятия эффектов шагов от донора.

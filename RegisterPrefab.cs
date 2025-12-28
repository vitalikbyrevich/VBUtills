namespace VBUtills
{
    public static class RegisterPrefab
    {
        public static void VB_AddPrefab(string bundleName, params string[] prefabNames)
        {
            foreach (var name in prefabNames)
            {
                var prefab = AssetBundleGet.LoadPrefabFromBundles<GameObject>(bundleName, name);
                if (prefab)
                {
                    PrefabManager.Instance.AddPrefab(prefab);
                    ShaderFix.Replace(prefab);
                }
            }
        }

        public static void VB_AddItem(string bundleName, params string[] prefabNames)
        {
            foreach (var name in prefabNames)
            {
                var prefab = AssetBundleGet.LoadPrefabFromBundles<GameObject>(bundleName, name);
                if (prefab)
                {
                    ItemManager.Instance.AddItem(new CustomItem(prefab, fixReference: true) { Recipe = null });
                    ShaderFix.Replace(prefab);
                }
            }
        }

        public static void VB_AddItemRecipe(string bundleName, params (string prefabName, int amountitem, (string item, int amount, int upgrade, bool recover)[] requirements, string craftingStation, int minLevel)[] items)
        {
            foreach (var item in items)
            {
                var prefab = AssetBundleGet.LoadPrefabFromBundles<GameObject>(bundleName, item.prefabName);
                if (!prefab) continue;

                var config = new ItemConfig
                {
                    CraftingStation = item.craftingStation,
                    Amount = item.amountitem,
                    Enabled = true,
                    MinStationLevel = item.minLevel,
                    Requirements = item.requirements.Select(r => new RequirementConfig
                        {
                            Item = r.item,
                            Amount = r.amount,
                            AmountPerLevel = r.upgrade,
                            Recover = r.recover
                        })
                        .ToArray()
                };

                ItemManager.Instance.AddItem(new CustomItem(prefab, fixReference: true, config));
                ShaderFix.Replace(prefab);
            }
        }
        
      /*  public static void VB_AddCreature(string bundleName, params string[] prefabNames)
        {
            foreach (var name in prefabNames)
            {
                var prefab = AssetBundleGet.LoadPrefabFromBundles<GameObject>(bundleName, name);
                if (prefab)
                {
                    CreatureManager.Instance.AddCreature(new CustomCreature(prefab, fixReference: true, new CreatureConfig { }));
                    ShaderFix.Replace(prefab);
                }
            }
        }*/
        
        public static void VB_AddCreatureWithConfig(string bundleName, params (string prefabName, CreatureConfig config)[] creatures)
        {
            foreach (var creature in creatures)
            {
                var prefab = AssetBundleGet.LoadPrefabFromBundles<GameObject>(bundleName, creature.prefabName);
                if (prefab)
                {
                    CreatureManager.Instance.AddCreature(new CustomCreature(prefab, fixReference: true, creature.config));
                    ShaderFix.Replace(prefab);
                }
            }
        }

        public static void VB_AddSE(string bundleName, params string[] prefabNames)
        {
            foreach (var name in prefabNames)
            {
                var statusEffect = AssetBundleGet.LoadPrefabFromBundles<StatusEffect>(bundleName, name);
                if (statusEffect)
                {
                    ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(statusEffect, fixReference: true));
                }
            }
        }

        public static void VB_AddPiece(string bundleName, string pieceTable, string category, string craftingStation, params (string prefabName, (string item, int amount, bool recover)[] requirements)[] pieces)
        {
            foreach (var piece in pieces)
            {
                var prefab = AssetBundleGet.LoadPrefabFromBundles<GameObject>(bundleName, piece.prefabName);
                if (!prefab) continue;

                var reqConfigs = piece.requirements.Select(r => new RequirementConfig
                {
                    Item = r.item,
                    Amount = r.amount,
                    AmountPerLevel = 0,
                    Recover = r.recover
                }).ToArray();

                var config = new PieceConfig
                {
                    PieceTable = pieceTable,
                    Category = category,
                    CraftingStation = craftingStation,
                    Requirements = reqConfigs,
                    Enabled = true,
                };

                PieceManager.Instance.AddPiece(new CustomPiece(prefab, true, config));
                ShaderFix.Replace(prefab);
            }
        }
    }
}
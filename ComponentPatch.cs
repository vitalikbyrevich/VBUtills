namespace VBUtills;

public static class ComponentPatch
{
     public static void Copy<T>(this T target, T source, Expression<Func<T, object>> fieldExpr)
     {
         var memberExpr = fieldExpr.Body as MemberExpression;
         if (memberExpr == null) return;
        
         var field = memberExpr.Member as FieldInfo;
         if (field == null) return;
        
         field.SetValue(target, field.GetValue(source));
     }
     
    public static void Replace<T>(string mob, string vanillaprefab, Action<T, T> replacer) where T : Component
    {
        var prefab = ZNetScene.instance.GetPrefab(mob);
        var vanilla = ZNetScene.instance.GetPrefab(vanillaprefab);
        if (!prefab || !vanilla) return;

        var comp = prefab.GetComponent<T>();
        var vanillaComp = vanilla.GetComponent<T>();
        if (comp && vanillaComp) replacer(comp, vanillaComp);
    }

    public static void AddEffect<T>(string mob, string effectPrefabName, Func<T, EffectList> selector) where T : Component
    {
        var prefab = ZNetScene.instance.GetPrefab(mob);
        if (!prefab) return;

        var component = prefab.GetComponent<T>();
        if (!component) return;

        var effectPrefab = ZNetScene.instance.GetPrefab(effectPrefabName);
        if (!effectPrefab) return;

        var effect = new EffectList.EffectData
        {
            m_prefab = effectPrefab,
            m_enabled = true,
            m_variant = -1
        };

        var effectList = selector(component);
        effectList.m_effectPrefabs = effectList.m_effectPrefabs?.Concat(new[] { effect }).ToArray() ?? new[] { effect };
    }
    
    public static void ReplaceAnimator(string customMobName, string vanillaMobName)
    {
        var customPrefab = ZNetScene.instance.GetPrefab(customMobName);
        var vanillaPrefab = ZNetScene.instance.GetPrefab(vanillaMobName);
    
        if (!customPrefab || !vanillaPrefab) return;

        var customAnimator = customPrefab.GetComponentInChildren<Animator>();
        var vanillaAnimator = vanillaPrefab.GetComponentInChildren<Animator>();
    
        if (customAnimator && vanillaAnimator)
        {
            customAnimator.runtimeAnimatorController = vanillaAnimator.runtimeAnimatorController;
            customAnimator.avatar = vanillaAnimator.avatar;
            customAnimator.applyRootMotion = vanillaAnimator.applyRootMotion;
        }
    }
    
    public static void ReplaceFoot(string targetPrefabName, string sourcePrefabName)
    {
        var targetPrefab = ZNetScene.instance.GetPrefab(targetPrefabName);
        var sourcePrefab = ZNetScene.instance.GetPrefab(sourcePrefabName);
        if (!targetPrefab || !sourcePrefab)
        {
            Debug.LogError($"Prefabs not found: Target={targetPrefabName}, Source={sourcePrefabName}");
            return;
        }

        var targetFootStep = targetPrefab.GetComponent<FootStep>();
        var sourceFootStep = sourcePrefab.GetComponent<FootStep>();
        if (!targetFootStep || !sourceFootStep)
        {
            Debug.LogError($"FootStep component missing: Target={!targetFootStep}, Source={!sourceFootStep}");
            return;
        }
        targetFootStep.m_effects = sourceFootStep.m_effects.Select(e => new StepEffect
            {
                m_name = e.m_name,
                m_motionType = e.m_motionType,
                m_material = e.m_material,
                m_effectPrefabs = e.m_effectPrefabs?.ToArray()
            })
            .ToList();
        
        targetFootStep.enabled = false;
        targetFootStep.enabled = true;

        Debug.Log($"Full FootStep effects replaced for {targetPrefabName} (from {sourcePrefabName})");
    }
}
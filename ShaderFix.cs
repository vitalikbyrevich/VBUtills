namespace VBUtills
{
	public static class ShaderFix
	{
		private static List<GameObject> _listprefab;
		private static bool _shaderFixPatched;

		static  ShaderFix()
		{
			_listprefab = new List<GameObject>();
			if (!_shaderFixPatched)
			{
				new Harmony("VitByr.VBUtills.ShaderFix").Patch(AccessTools.DeclaredMethod(typeof(FejdStartup), nameof(FejdStartup.Awake)), null, new HarmonyMethod(AccessTools.DeclaredMethod(typeof(ShaderFix), nameof(ShaderFix.ReplaceShaderPatch))));
				_shaderFixPatched = true;
			}
			_listprefab.Clear();
		}

		public static void Replace(GameObject gameObject) => _listprefab.Add(gameObject);
		
	/*	public static void ReplacePack(params GameObject[] gameObjects)
		{
			if (gameObjects == null) return;
			foreach (var go in gameObjects) if (go) GOToSwap.Add(go);
		}*/
		
		[HarmonyPriority(700)]
		private static void ReplaceShaderPatch()
		{
			foreach (Material item in from gameObject in _listprefab
			         select gameObject.GetComponentsInChildren<Renderer>(includeInactive: true) into renderers
			         from renderer in renderers
			         where renderer
			         from material in renderer.sharedMaterials
			         where material
			         select material)
			{
				Shader[] array = Resources.FindObjectsOfTypeAll<Shader>();
				foreach (Shader shader in array) if (!(item.shader.name == "Standard") && item.shader.name == shader.name) item.shader = shader;
			}
		}
	}
}
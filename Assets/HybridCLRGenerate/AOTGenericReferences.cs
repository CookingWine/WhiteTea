using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"GameFramework.dll",
		"UnityGameFramework.Runtime.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// }}

	public void RefMethods()
	{
		// System.Void GameFramework.GameFrameworkLog.Warning<object>(string,object)
		// string GameFramework.Utility.Text.Format<object>(string,object)
		// string GameFramework.Utility.Text.ITextHelper.Format<object>(string,object)
		// System.Void UnityGameFramework.Runtime.Log.Warning<object>(string,object)
	}
}
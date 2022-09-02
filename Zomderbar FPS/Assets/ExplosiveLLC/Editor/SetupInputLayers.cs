using UnityEditor;
using UnityEngine;

namespace RPGCharacterAnims
{
	class SetupInputLayers:AssetPostprocessor
	{
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			SetupMessageWindow window = ( SetupMessageWindow )EditorWindow.GetWindow(typeof(SetupMessageWindow), true, "Load Input and Tag Presets");
			window.maxSize = new Vector2(320f, 145f);
			window.minSize = window.maxSize;
		}
	}

	public class SetupMessageWindow:EditorWindow
	{
		void OnGUI()
		{
			EditorGUILayout.LabelField("Before attempting to use the RPG Character Animation Mecanim Animation Pack FREE, you must first ensure that the inputs and layers are correctly defined.\n \nThere are included Inputs.preset and TagLayers.preset files which contain all the settings that you can load in.\n \nYou can remove this message by deleting the SetupInputLayers.cs script in the Editor folder.", EditorStyles.wordWrappedLabel);
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Com.TheFallenGames.OSA.Core;

namespace Com.TheFallenGames.OSA.Editor
{
    static class MenuItems
	{
		[MenuItem("Tools/OSA/Code reference")]
		public static void OpenOnlineCodeReference()
		{ Application.OpenURL("http://thefallengames.com/unityassetstore/optimizedscrollviewadapter/doc"); }

		[MenuItem("Tools/OSA/Manual and FAQ")]
		public static void OpenOnlineManual()
		{ Application.OpenURL("https://docs.google.com/document/d/1exc3hz9cER9fKx2m0rXxTG0-vMxEGdFrd1NYdDJuATk/edit?usp=sharing"); }

		[MenuItem("Tools/OSA/Quick start video (old, but still quite useful)")]
		public static void OpenTutorial()
		{ Application.OpenURL("https://youtu.be/rcgnF16JybY"); }

		[MenuItem("Tools/OSA/OSA wizard video")]
		public static void OpenWizardVideo()
		{ Application.OpenURL("https://youtu.be/BnA32GV13ws"); }

		[MenuItem("Tools/OSA/Changelog")]
		public static void OpenOnlineChangelog()
		{ Application.OpenURL("http://thefallengames.com/unityassetstore/optimizedscrollviewadapter/Changelog.txt"); }

		[MenuItem("Tools/OSA/Thank you!")]
		public static void OpenThankYou()
		{ Application.OpenURL("http://thefallengames.com/unityassetstore/optimizedscrollviewadapter/thankyou"); }

		[MenuItem("Tools/OSA/Ask us a question")]
		public static void AskQuestion()
		{ Application.OpenURL("https://forum.unity.com/threads/30-off-optimized-scrollview-adapter-listview-gridview.395224"); }

		[MenuItem("Tools/OSA/About")]
		public static void OpenAbout()
		{
			EditorUtility.DisplayDialog(
				"OSA " + C.OSA_VERSION_STRING,
				"May the result of our passion and hard work aid you along your journey in creating something marvellous!" +
				"\r\n\r\nOptimized ScrollView Adapter by The Fallen Games" +
				"\r\nlucian@thefallengames.com" +
				"\r\ngeorge@thefallengames.com",
				"Close"
			);
		}

		[MenuItem("CONTEXT/ScrollRect/Optimize with OSA")]
		static void OptimizeSelectedScrollRectWithOSA(MenuCommand command)
		{
			ScrollRect scrollRect = (ScrollRect)command.context;
			var validationResult = InitOSAWindow.Validate(true, scrollRect);
			// Manually checking for validation, as this provides richer info about the case when initialization is not possible
			if (!validationResult.isValid)
			{
				CWiz.ShowCouldNotExecuteCommandNotification(null);
				Debug.Log("OSA: Could not optimize '" + scrollRect.name + "': " + validationResult.reasonIfNotValid);
				return;
			}

			InitOSAWindow.Open(InitOSAWindow.Parameters.Create(validationResult));
		}

		[MenuItem("GameObject/UI/Optimized ScrollView (OSA)", false, 10)]
		static void CreateOSA(MenuCommand menuCommand)
		{

			string reasonIfNotValid;
			// Manually checking for validation, as this provides richer info about the case when creation is not possible
			if (!CreateOSAWindow.Validate(true, out reasonIfNotValid))
			{
				CWiz.ShowCouldNotExecuteCommandNotification(null);
				Debug.Log("OSA: Could not create ScrollView on the selected object: " + reasonIfNotValid);
				return;
			}

			CreateOSAWindow.Open(new CreateOSAWindow.Parameters());
		}
	}
}

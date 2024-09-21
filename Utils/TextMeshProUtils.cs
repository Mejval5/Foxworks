using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Foxworks.Utils
{
	/// <summary>
	/// The purpose of this utility class is to reduce the boilerplate code and simplify the Vector3 operations.
	/// </summary>
	public static class TextMeshProUtils
	{
		/// <summary>
		/// Returns a link if pointer is clicked on it
		/// </summary>
		/// <param name="linkInfo">Returned link on success</param>
		/// <param name="textMeshPro">Text mesh Pro source</param>
		/// <param name="disableHiddenLink">Set to false if you want links to react when not visible.</param>
		/// <returns>True when clicking on a link</returns>
		public static bool GetPointedLinkInfo(out TMP_LinkInfo linkInfo, TextMeshProUGUI textMeshPro, bool disableHiddenLink = true)
		{
			if (disableHiddenLink && Mathf.Approximately(textMeshPro.alpha, 0f))
			{
				// LinkInfo cannot be null before exiting method
				linkInfo = new TMP_LinkInfo();
				return false;
			}

			// Try to get from Screen space overlay
			int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, null);

			// If no linkIndex found, try to search on every cameras
			for (int i = 0; linkIndex < 0 && i < Camera.allCamerasCount; i++)
			{
				linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, Camera.allCameras[i]);

				if (linkIndex > -1)
				{
					Debug.Log($"[[GetPointedTMPLinkInfo]], link found on camera {i} which is " + (Camera.allCameras[i] != Camera.current ? "not " : "") + "current and " + (Camera.allCameras[i] != Camera.main ? "not " : "") + "main");
				}
			}

			// Click is not on a link
			if (linkIndex < 0)
			{
				// LinkInfo cannot be null before exiting method
				linkInfo = new TMP_LinkInfo();
				return false;
			}

			// Set out variable as clicked link
			linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
			return true;
		}

		public static TMP_InputField GetTMPInputField(this BaseEventData eventData)
		{
			return eventData?.selectedObject.GetComponent<TMP_InputField>();
		}
	}
}
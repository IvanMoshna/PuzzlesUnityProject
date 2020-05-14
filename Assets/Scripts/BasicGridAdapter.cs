/*
 * * * * This bare-bones script was auto-generated * * * *
 * The code commented with "/ * * /" demonstrates how data is retrieved and passed to the adapter, plus other common commands. You can remove/replace it once you've got the idea
 * Complete it according to your specific use-case
 * Consult the Example scripts if you get stuck, as they provide solutions to most common scenarios
 * 
 * Main terms to understand:
 *		Model = class that contains the data associated with an item (title, content, icon etc.)
 *		Views Holder = class that contains references to your views (Text, Image, MonoBehavior, etc.)
 * 
 * Default expected UI hiererchy:
 *	  ...
 *		-Canvas
 *		  ...
 *			-MyScrollViewAdapter
 *				-Viewport
 *					-Content
 *				-Scrollbar (Optional)
 *				-ItemPrefab (Optional)
 * 
 * Note: If using Visual Studio and opening generated scripts for the first time, sometimes Intellisense (autocompletion)
 * won't work. This is a well-known bug and the solution is here: https://developercommunity.visualstudio.com/content/problem/130597/unity-intellisense-not-working-after-creating-new-1.html (or google "unity intellisense not working new script")
 * 
 * 
 * Please read the manual under "Assets/OSA/Docs", as it contains everything you need to know in order to get started, including FAQ
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Other.Extensions;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using Common;
using Random = System.Random;

// The date was temporarily included in the namespace to prevent duplicate class names
// You should modify the namespace to your own or - if you're sure there will be no conflicts - remove it altogether
namespace Your.Namespace.Here20May12044942767.Grids
{
	// There is 1 important callback you need to implement: UpdateCellViewsHolder()
	// See explanations below
	public class BasicGridAdapter : GridAdapter<GridParams, MyGridItemViewsHolder>
	{
		// Helper that stores data and notifies the adapter when items count changes
		// Can be iterated and can also have its elements accessed by the [] operator
		public SimpleDataHelper<PuzzleSource> Data { get; private set; }

		public Sprite[] patternsSprites;
		
		#region OSA implementation
		protected override void Start()
		{
			base.Start();
			var content = ToolBox.Get<Content>();
			Data = new SimpleDataHelper<PuzzleSource>(this);
			Data.InsertItemsAtStart(content.puzzleSources);
			// Calling this initializes internal data and prepares the adapter to handle item count changes
			
			/*List<Sprite> iconsColor = new List<Sprite>();
			for (int i = 0; i < content.puzzleSources.Count; i++)
			{
				var pathColor = Path.Combine("Color", content.puzzleSources[i].originalImageID);
				iconsColor.Add(Resources.Load<Sprite>(pathColor));
			}*/

			// Retrieve the models from your data source and set the items count

			RetrieveDataAndUpdate(Data.Count);
			
		}

		// This is called anytime a previously invisible item become visible, or after it's created, 
		// or when anything that requires a refresh happens
		// Here you bind the data from the model to the item's views
		// *For the method's full description check the base implementation
		protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
		{
			// In this callback, "newOrRecycled.ItemIndex" is guaranteed to always reflect the
			// index of item that should be represented by this views holder. You'll use this index
			// to retrieve the model from your data set
			
			PuzzleSource model = Data[newOrRecycled.ItemIndex];
			//ЗДЕСЬ ПИХАЕМ КАРТИНКИ




			//newOrRecycled.text.text = model.originalImageID;
			var pathColor = Path.Combine("Color", model.originalImageID);
			var pathBW = Path.Combine("BW", model.backgroundImageID);
			//Debug.Log(pathColor);
			Sprite colorSprite = Resources.Load<Sprite>(pathColor);
			Sprite bwSprite = Resources.Load<Sprite>(pathBW);
			//newOrRecycled.icon.sprite = colorSprite;
			newOrRecycled.backgroundIcon.sprite = bwSprite;
			newOrRecycled.pattenIcon.sprite = patternsSprites[UnityEngine.Random.Range(0, patternsSprites.Length)];
			newOrRecycled.pattenIcon.SetNativeSize();
			var icon = newOrRecycled.pattenIcon.transform.GetChild(0);
			icon.GetComponent<Image>().sprite = colorSprite;

			//newOrRecycled.backgroundImage.color = model.color;
			//newOrRecycled.titleText.text = model.title + " #" + newOrRecycled.ItemIndex;

		}

		// This is the best place to clear an item's views in order to prepare it from being recycled, but this is not always needed, 
		// especially if the views' values are being overwritten anyway. Instead, this can be used to, for example, cancel an image 
		// download request, if it's still in progress when the item goes out of the viewport.
		// <newItemIndex> will be non-negative if this item will be recycled as opposed to just being disabled
		// *For the method's full description check the base implementation
		/*
		protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
		{
			base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
		}
		*/
		#endregion

		// These are common data manipulation methods
		// The list containing the models is managed by you. The adapter only manages the items' sizes and the count
		// The adapter needs to be notified of any change that occurs in the data list. 
		// For GridAdapters, only Refresh and ResetItems work for now
		#region data manipulation
		public void AddItemsAt(int index, IList<PuzzleSource> items)
		{
			//Commented: this only works with Lists. ATM, Insert for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			//Data.InsertItems(index, items);
			Data.List.InsertRange(index, items);
			Data.NotifyListChangedExternally();
		}

		public void RemoveItemsFrom(int index, int count)
		{
			//Commented: this only works with Lists. ATM, Remove for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			//Data.RemoveRange(index, count);
			Data.List.RemoveRange(index, count);
			Data.NotifyListChangedExternally();
		}

		public void SetItems(IList<PuzzleSource> items)
		{
			Data.ResetItems(items);
		}
		#endregion


		// Here, we're requesting <count> items from the data source  Здесь мы запрашиваем <count> элементы из источника данных
		public void RetrieveDataAndUpdate(int count)
		{
			StartCoroutine(FetchMoreItemsFromDataSourceAndUpdate(count));
		}

		// Retrieving <count> models from the data source and calling OnDataRetrieved after.
		// In a real case scenario, you'd query your server, your database or whatever is your data source and call OnDataRetrieved after
		IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
		{
			// Simulating data retrieving delay
			yield return new WaitForSeconds(.5f);
			
			var newItems = new PuzzleSource[count];

			// Retrieve your data here
			
			for (int i = 0; i < count; ++i)
			{
				var model = Data[i];
				{
					model.originalImageID = Data[i].originalImageID;
					model.backgroundImageID = Data[i].backgroundImageID;
				}
			
				newItems[i] = model;
			}
			

			OnDataRetrieved(newItems);//здесь пустой список     теперь не пустой
		}

		void OnDataRetrieved(PuzzleSource[] newItems)//добавляет итемы в контент
		{
			//Commented: this only works with Lists. ATM, Insert for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			// Data.InsertItemsAtEnd(newItems);

			for (int i = 0; i < Data.Count; i++)
			{
				
			}
			
			Data.List.AddRange(newItems);
			Data.NotifyListChangedExternally();
		}
	}


	// Class containing the data associated with an item
	public class MyGridItemModel
	{
		public Image icon;
		public Image backIcon;
	}


	// This class keeps references to an item's views. //Этот класс хранит ссылки на представления элемента.
	// Your views holder should extend BaseItemViewsHolder for ListViews and CellViewsHolder for GridViews
	// The cell views holder should have a single child (usually named "Views"), which contains the actual 
	// UI elements. A cell's root is never disabled - when a cell is removed, only its "views" GameObject will be disabled
	public class MyGridItemViewsHolder : CellViewsHolder
	{
		
		//public Image icon;
		public Image backgroundIcon;
		//public Text text;
		public Image pattenIcon;
		


		// Retrieving the views from the item's root GameObject
		public override void CollectViews()
		{
			base.CollectViews();

			// GetComponentAtPath is a handy extension method from frame8.Logic.Misc.Other.Extensions
			// which infers the variable's component from its type, so you won't need to specify it yourself
			
			//views.GetComponentAtPath("Icon", out icon);
			views.GetComponentAtPath("BackgroundIcon", out backgroundIcon);
			views.GetComponentAtPath("PatternIcon", out pattenIcon);
			//views.GetComponentAtPath("Text", out text);
			
			
			
			
			
		}
		
		// This is usually the only child of the item's root and it's called "Views". 
		// That's what the default implementation will look for, but just for flexibility, 
		// this callback is provided, in case it's named differently or there's more than 1 child 
		// *See GridExample.cs for more info
		/*
		protected override RectTransform GetViews()
		{ return root.Find("Views").transform as RectTransform; }
		*/

		// Override this if you have children layout groups. They need to be marked for rebuild when this callback is fired
		/*
		public override void MarkForRebuild()
		{
			base.MarkForRebuild();

			LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout1);
			LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout2);
		}
		*/
	}
}

//#define DEBUG_COMPUTE_VISIBILITY

using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.TheFallenGames.OSA.Core.SubComponents
{
	internal class NestingManager<TParams, TItemViewsHolder> : IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
		where TParams : BaseParams
		where TItemViewsHolder : BaseItemViewsHolder
	{
		public bool SearchedParentAtLeastOnce { get { return _SearchedAtLeastOnce; } }
		public bool CurrentDragCapturedByParent { get { return _CurrentDragCapturedByParent; } }


		OSA<TParams, TItemViewsHolder> _Adapter;
		InternalState<TItemViewsHolder> _InternalState;
		IInitializePotentialDragHandler initializePotentialDragHandler;
		IBeginDragHandler beginDragHandler;
		IDragHandler dragHandler;
		IEndDragHandler endDragHandler;
		bool _SearchedAtLeastOnce;
		bool _CurrentDragCapturedByParent;


		public NestingManager(OSA<TParams, TItemViewsHolder> adapter)
		{
			_Adapter = adapter;
			_InternalState = _Adapter._InternalState;
		}


		public bool FindAndStoreNestedParent()
		{
			initializePotentialDragHandler = null;
			beginDragHandler = null;
			dragHandler = null;
			endDragHandler = null;

			var tr = _Adapter.transform;
			// Find the first parent that implements all of the interfaces
			while ((tr = tr.parent) && initializePotentialDragHandler == null)
			{
				initializePotentialDragHandler = tr.GetComponent(typeof(IInitializePotentialDragHandler)) as IInitializePotentialDragHandler;
				if (initializePotentialDragHandler == null)
					continue;

				beginDragHandler = initializePotentialDragHandler as IBeginDragHandler;
				if (beginDragHandler == null)
				{
					initializePotentialDragHandler = null;
					continue;
				}

				dragHandler = initializePotentialDragHandler as IDragHandler;
				if (dragHandler == null)
				{
					initializePotentialDragHandler = null;
					beginDragHandler = null;
					continue;
				}

				endDragHandler = initializePotentialDragHandler as IEndDragHandler;
				if (endDragHandler == null)
				{
					initializePotentialDragHandler = null;
					beginDragHandler = null;
					dragHandler = null;
					continue;
				}
			}

			_SearchedAtLeastOnce = true;

			return initializePotentialDragHandler != null;
		}

		public void OnInitializePotentialDrag(PointerEventData eventData)
		{
			if (!_SearchedAtLeastOnce && !FindAndStoreNestedParent())
				return;

			if (initializePotentialDragHandler == null)
				return;

			initializePotentialDragHandler.OnInitializePotentialDrag(eventData);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (initializePotentialDragHandler == null)
			{
				_CurrentDragCapturedByParent = false;
				return;
			}

			if (_Adapter.Parameters.DragEnabled)
			{
				var delta = eventData.delta;
				float dyExcess = Mathf.Abs(delta.y) - Mathf.Abs(delta.x);

				_CurrentDragCapturedByParent = _InternalState.hor1_vertMinus1 * dyExcess >= 0f; // parents have priority when dx == dy, since they are supposed to be more important
			}
			else
				// When the child ScrollView has its drag disabled, forward the drag to the parent no matter what
				_CurrentDragCapturedByParent = true;

			if (!_CurrentDragCapturedByParent)
				return;

			beginDragHandler.OnBeginDrag(eventData);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (initializePotentialDragHandler == null)
				return;

			dragHandler.OnDrag(eventData);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (initializePotentialDragHandler == null)
				return;

			endDragHandler.OnEndDrag(eventData);
			_CurrentDragCapturedByParent = false;
		}
	}
}

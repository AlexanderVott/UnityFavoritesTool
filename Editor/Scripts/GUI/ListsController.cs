using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool
{
    public class ListsController : BaseController
    {
	    private ScrollView _listsScroll;

		internal ListsController(VisualElement root) : base(root)
		{
			_listsScroll = _panel.Q<ScrollView>("listsScroll");
		}
    }
}

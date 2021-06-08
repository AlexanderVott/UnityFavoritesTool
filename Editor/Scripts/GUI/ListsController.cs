using UnityEngine.UIElements;

namespace FavTool
{
	[ControllerTemplate("ListsControllerTemplate")]
	public class ListsController : BaseController
    {
	    private ScrollView _listsScroll;

		internal ListsController(VisualElement root) : base(root)
		{
			_listsScroll = _panel.Q<ScrollView>("listsScroll");
		}
    }
}

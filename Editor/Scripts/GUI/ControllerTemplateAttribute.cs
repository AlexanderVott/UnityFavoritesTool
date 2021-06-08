using System;

namespace FavTool
{
    internal class ControllerTemplateAttribute : Attribute
    {
	    internal string path;

	    internal ControllerTemplateAttribute(string path)
	    {
		    this.path = path;
	    }
    }
}

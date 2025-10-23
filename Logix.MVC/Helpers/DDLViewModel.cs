using Microsoft.AspNetCore.Mvc.Rendering;

namespace Logix.MVC.Helpers
{
    public class DDLViewModel
    {
        private readonly Dictionary<string, SelectList> _selectLists = new Dictionary<string, SelectList>();
        public void AddList(string propertyName, SelectList list)
        {
            _selectLists.Add(propertyName, list);
        }

        public SelectList GetSelectList(string propertyName)
        {
            if (!_selectLists.ContainsKey(propertyName))
            {
                throw new ArgumentException($"Property {propertyName} not found on view model.");
            }

            return _selectLists[propertyName];
        }

        public Dictionary<string, SelectList> AllLists { get { return _selectLists; } } 
    }
}

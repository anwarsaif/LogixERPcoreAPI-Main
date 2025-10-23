namespace Logix.MVC.Helpers
{
    public class BindListHelper
    {
        private readonly Dictionary<string, object> _lists = new Dictionary<string, object>();

        public void AddList<T>(string propertyName, T list)
        {
            _lists.Add(propertyName, list);
        }

        public T GetList<T>(string propertyName)
        {
            if (!_lists.ContainsKey(propertyName))
            {
                throw new ArgumentException($"Property {propertyName} not found on view model.");
            }

            return (T)_lists[propertyName];
        }
    }
}

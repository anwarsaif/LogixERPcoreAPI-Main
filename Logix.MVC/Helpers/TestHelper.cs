namespace Logix.MVC.Helpers
{
    public class TestHelper
    {
    }
    public class secClass<T>
    {
        // Properties of secClass
        public string Property1 { get; set; }
        public T Property2 { get; set; }
    }

    public class MyClassService
    {
        public List<secClass<T>> ConvertToSecClass<T, U>(IEnumerable<U> inputList, string prop1Name, string prop2Name)
        {
            var secClassList = new List<secClass<T>>();

            foreach (var item in inputList)
            {
                var property1Value = item.GetType().GetProperty(prop1Name)?.GetValue(item, null);
                var property2Value = item.GetType().GetProperty(prop2Name)?.GetValue(item, null);

                var secClassObj = new secClass<T>
                {
                    Property1 = property1Value != null ? property1Value.ToString() : string.Empty,
                    Property2 = property2Value != null ? (T)property2Value : default(T)
                };

                secClassList.Add(secClassObj);
            }

            return secClassList;
        }
    }
}

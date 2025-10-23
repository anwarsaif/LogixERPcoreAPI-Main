namespace Logix.MVC.DataTableModel
{
    public class DataTableRequestModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public SearchModel search { get; set; }
        public List<OrderModel> order { get; set; }
        public List<ColumnModel> columns { get; set; }
    }

    public class SearchModel
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }

    public class OrderModel
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class ColumnModel
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public SearchModel search { get; set; }
    }

    public class DataTableResponseModel<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
    }

}

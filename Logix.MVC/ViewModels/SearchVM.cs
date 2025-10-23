namespace Logix.MVC.ViewModels
{
    public class SearchVM<TSearch, TList> where TSearch : class
    {
        public SearchVM(TSearch search, IEnumerable<TList> list)
        {
            SearchModel = search;
            ListModel = list;
        }
        public TSearch SearchModel { get; set; }
        public IEnumerable<TList> ListModel { get; set; }

        //using this properties in Print
        public string? FacilityName { get; set; }
        public string? FacilityName2 { get; set; }
        public string? FacilityAddress { get; set; }
        public string? FacilityMobile { get; set; }
        public string? FacilityLogoPrint { get; set; }
        public string? UserName { get; set; }
        public string? FacilityFooterPrint { get; set; }


    }
}

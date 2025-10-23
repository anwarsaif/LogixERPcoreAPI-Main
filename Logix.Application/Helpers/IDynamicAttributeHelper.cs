using Logix.Application.Common;
using Logix.Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Http;
namespace Logix.Application.Helpers
{
    public interface IDynamicAttributeHelper
    {
        Task<bool> GetAttributeWithValue(long ScreenID, long AppId);
    }

    public class DynamicAttributeHelper : IDynamicAttributeHelper
    {
        private readonly IWFRepositoryManager wFRepositoryManager;
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly IHrRepositoryManager hrRepositoryManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentData session;

        public DynamicAttributeHelper(
            IHttpContextAccessor httpContextAccessor,
            ICurrentData session,
            IWFRepositoryManager wFRepositoryManager,
            IMainRepositoryManager mainRepositoryManager,
            IHrRepositoryManager hrRepositoryManager
        )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.session = session;
            this.wFRepositoryManager = wFRepositoryManager;
            this.mainRepositoryManager = mainRepositoryManager;
            this.hrRepositoryManager = hrRepositoryManager;
        }

        public async Task<bool> GetAttributeWithValue(long screenId, long appId)
        {
            try
            {
                var data = await mainRepositoryManager.SysDynamicAttributeRepository.GetDynamicAttributeData(screenId, appId); // Example

                foreach (var row in data)
                {
                    Guid dynamicAttributeId = row.DynamicAttributeId;
                    DataTypeIdEnum dataTypeId = (DataTypeIdEnum)row.DataTypeId;
                    string attributeName;
                    if (session.Language == 1)
                    {
                        attributeName = (string)row.AttributeName;
                    }
                    else
                    {
                        attributeName = (string)row.AttributeName2;
                    }
                    bool attributeRequired = (bool)row.Required;
                    int lookUpCatagoriesId = (int)row.LookUp_Catagories_ID;

                    object attributeValue = row.DynamicValue;


                //    AddCustomAttributeForUpdate(dynamicAttributeId, dataTypeId,
                //                                attributeName, attributeRequired,
                //                                lookUpCatagoriesId, attributeValue);
                //
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //private void AddCustomAttributeForUpdate(Guid dynamicAttributeId, DataTypeIdEnum dataType, string attributeName, bool attributeRequired, int lookUpCategoriesId, object attributeValue)
        //{
        //    // Create a DIV element
        //    var div = new  System.Web.UI.HtmlControls.HtmlGenericControl("div", new { @class = "row-fluid" });

        //    // Create another DIV element
        //    var div2 = new HtmlElement("div", new { @class = "span12" });

        //    // Set content based on data type
        //    string contentDiv2;
        //    if (dataType == DataTypeIdEnum.Title)
        //    {
        //        contentDiv2 = $"<h3>class='header smaller lighter blue'>{attributeName}</h3>";
        //    }
        //    else
        //    {
        //        contentDiv2 = $"<label class='control-label'>{attributeName}</label>";
        //    }
        //    div2.InnerHtml = contentDiv2;

        //    // Add div2 to div
        //    div.AppendChild(div2);

        //    // Create another DIV element for controls
        //    var div3 = new HtmlElement("div", new { @class = "controls" });

        //    // Add UI elements if not Title type (implementation not shown)
        //    if (dataType != DataTypeIdEnum.Title)
        //    {
        //        // Call method to create UI elements (assuming it returns a list of HtmlElement)
        //        var uiControls = CreateCustomAttributeUIForUpdate(dynamicAttributeId, dataType, attributeRequired, lookUpCategoriesId, attributeValue);

        //        // Add each UI element to div3
        //        foreach (var ctrl in uiControls)
        //        {
        //            div3.AppendChild(ctrl);
        //        }
        //    }

        //    // Add div3 to div
        //    div.AppendChild(div3);

        //    // Add the entire div to the CustomUITable control (assuming it's an HtmlElement)
        //    CustomUITable.AppendChild(div);
        //}

        //private void AddCustomAttributeForUpdate(Guid DynamicAttributeId, DataTypeIdEnum DataTypeId, string AttributeName, bool AttributeRequired, int LookUp_Catagories_ID, object AttributeValue)
        //{
        //    HtmlGenericControlTagHelper Div = new HtmlGenericControlTagHelper("div");
        //    Div.Attributes.Add("class", "row-fluid");

        //    HtmlGenericControlTagHelper Div2 = new HtmlGenericControlTagHelper("div");
        //    Div2.Attributes.Add("Class", "span12");

        //    string ContentDiv2 = "";
        //    if (DataTypeId == DataTypeIdEnum.Title)
        //    {
        //        ContentDiv2 = $"<h3 class='header smaller lighter blue'>{AttributeName}</h3>";
        //    }
        //    else
        //    {
        //        ContentDiv2 = $"<label class='control-label'>{AttributeName}</label>";
        //    }
        //    Div2.InnerHtml = ContentDiv2;

        //    Div.Content.Add(Div2);

        //    HtmlGenericControlTagHelper Div3 = new HtmlGenericControlTagHelper("div");
        //    Div3.Attributes.Add("class", "controls");

        //    if (DataTypeId != DataTypeIdEnum.Title)
        //    {
        //        List<ITagHelper> UIControls = CreateCustomAttributeUIForUpdate(DynamicAttributeId, DataTypeId, AttributeRequired, LookUp_Catagories_ID, AttributeValue);
        //        foreach (ITagHelper ctrl in UIControls)
        //        {
        //            Div3.Content.Add(ctrl);
        //        }
        //        Div.Content.Add(Div3);
        //    }

        //    CustomUITable.Content.Add(Div);
        //}
    }
}
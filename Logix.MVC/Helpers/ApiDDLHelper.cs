using Logix.Application.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace Logix.MVC.Helpers
{
    public interface IApiDDLHelper
    {
        Task<SelectList> GetAnyLis<TModel, TValue>(Expression<Func<TModel, bool>> where, string valueProperty, string nameProperty, bool hasDefault = false, string defaultText = "all", TValue defaultValue = default, TValue selectedValue = default) where TModel : class;
        Task<SelectList> GetAnyLis<TModel, TValue>(string valueProperty, string nameProperty, bool hasDefault = false, string defaultText = "all", TValue defaultValue = default, TValue selectedValue = default) where TModel : class;
        Task<SelectList> GetByCategoryId(int categoryId, int lang = 1, int selectedValue = 0, bool hasDefault = false, string defaultText = "all", int defaultValue = 0);


    }

    public class ApiDDLHelper : IApiDDLHelper
    {
        private readonly IDDListHelper listHelper;
        private readonly ILocalizationService localization;

        public ApiDDLHelper(IDDListHelper listHelper, ILocalizationService localization)
        {
            this.listHelper = listHelper;
            this.localization = localization;
        }
        public async Task<SelectList> GetByCategoryId(int categoryId, int lang = 1, int selectedValue = 0, bool hasDefault = false, string defaultText = "all", int defaultValue = 0)
        {
            var list = await listHelper.GetList(categoryId, lang: lang, selectedValue: selectedValue, hasDefault: hasDefault, defaultText: defaultText, defaultValue: defaultValue);
            return list;
        }

        public async Task<SelectList> GetAnyLis<TModel, TValue>(Expression<Func<TModel, bool>> where, string valueProperty, string nameProperty, bool hasDefault = false, string defaultText = "all", TValue defaultValue = default, TValue selectedValue = default) where TModel : class
        {
            if (!string.IsNullOrWhiteSpace(valueProperty) && !string.IsNullOrWhiteSpace(nameProperty))
            {
                var first = new DDLItem<TValue> { Value = defaultValue, Name = defaultText };
                var selectList = new List<DDLItem<TValue>>();
                if (hasDefault)
                {
                    selectList.Add(first);
                }
                try
                {
                    var list = await listHelper.GetRawList<TModel, TValue>(where, valueProperty, nameProperty);
                    if (list != null && list.Any())
                    {
                        selectList.AddRange(list);
                    }
                    else
                    {
                        if (hasDefault)
                            selectList[0].Name = "لا توجد بيانات";
                    }
                }
                catch (Exception)
                {
                    if (hasDefault)
                        selectList[0].Name = "خطأ! لا توجد بيانات";
                }

                return new SelectList(selectList, "Value", "Name", selectedValue);
            }
            else
            {
                throw new ArgumentException($"Exception in: (ApiDDLHelper -> GetAnyList) The valueProperty and nameProperty must have values");
            }
        }
        public async Task<SelectList> GetAnyLis<TModel, TValue>(string valueProperty, string nameProperty, bool hasDefault = false, string defaultText = "all", TValue defaultValue = default, TValue selectedValue = default) where TModel : class
        {
            if (!string.IsNullOrWhiteSpace(valueProperty) && !string.IsNullOrWhiteSpace(nameProperty))
            {
                var first = new DDLItem<TValue> { Value = defaultValue, Name = localization.GetCommonResource(defaultText) };
                var selectList = new List<DDLItem<TValue>>();
                if (hasDefault)
                {
                    selectList.Add(first);
                }
                try
                {
                    Expression<Func<TModel, bool>> where = model => true;
                    var list = await listHelper.GetRawList<TModel, TValue>(where, valueProperty, nameProperty);
                    if (list != null && list.Any())
                    {
                        selectList.AddRange(list);
                    }
                    else
                    {
                        if (hasDefault)
                            selectList[0].Name = "لا توجد بيانات";
                    }
                }
                catch (Exception)
                {
                    if (hasDefault)
                        selectList[0].Name = "خطأ! لا توجد بيانات";
                }

                return new SelectList(selectList, "Value", "Name", selectedValue);
            }
            else
            {
                throw new ArgumentException($"Exception in: (ApiDDLHelper -> GetAnyList) The valueProperty and nameProperty must have values");
            }
        }


    }
}

using Logix.Application.Common;
using Logix.Application.DTOs.Common;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace Logix.MVC.Helpers
{
    public interface IDDListHelper
    {
        Task<SelectList> GetList(int categoryId, int lang = 0, string textClm = "Name", string valueClm = "Code", int selectedValue = 0, bool hasDefault = true, string defaultText = "all", int defaultValue = 0);
        Task<List<DDLItem<TValue>>> GetRawList<TModel, TValue>(Expression<Func<TModel, bool>> where, string valueProperty, string nameProperty) where TModel : class;
        SelectList GetFromList<T>(IEnumerable<DDListItem<T>> list, string textClm = "Name", string valueClm = "Value", T selectedValue = default(T), bool hasDefault = true, string defaultText = "all", T defaultValue = default(T));

        Task<DDLResult<TValue>> GetRawListPagination<TModel, TValue>(Expression<Func<TModel, bool>> where, string valueProperty, string nameProperty, TValue lastSeenId, int pageSize = 10) where TModel : class where TValue : IComparable<TValue>;
    }



    public class DDListHelper : IDDListHelper
    {
        private readonly ISysLookupDataService sysLookup;
        private readonly ILocalizationService localization;
        private readonly IDDLService ddlService;

        public DDListHelper(ISysLookupDataService sysLookup, ILocalizationService localization, IDDLService ddlService)
        {
            this.sysLookup = sysLookup;
            this.localization = localization;
            this.ddlService = ddlService;
        }

        public SelectList GetFromList<T>(IEnumerable<DDListItem<T>> list, string textClm, string valueClm, T selectedValue = default(T), bool hasDefault = true, string defaultText = "all", T defaultValue = default(T))
        {
            var first = new DDListItem<T> { Value = defaultValue, Name = localization.GetCommonResource(defaultText) };
            var selectList = new List<DDListItem<T>>();
            if (hasDefault)
            {
                selectList.Add(first);
            }
            if (list != null && list.Any())
            {
                selectList.AddRange(list);
            }
            else
            {
                if (hasDefault)
                    selectList[0].Name = "لا توجد بيانات";
            }

            return new SelectList(selectList, valueClm, textClm, selectedValue);
        }

        public async Task<List<DDLItem<TValue>>> GetRawList<TModel, TValue>(Expression<Func<TModel, bool>> where, string valueProperty, string nameProperty) where TModel : class
        {
            var selectList = new List<DDLItem<TValue>>();
            try
            {
                var list = await ddlService.GetDDL<TModel, TValue>(valueProperty, nameProperty, where);
                if (list != null && list.Any())
                {
                    selectList.AddRange(list);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }

            return selectList;
        }

        public async Task<SelectList> GetList(int categoryId, int lang = 0, string textClm = "Name", string valueClm = "Code", int selectedValue = 0, bool hasDefault = true, string defaultText = "all", int defaultValue = 0)
        {
            try
            {
                var first = new DDItem { Code = defaultValue, Name = localization.GetCommonResource(defaultText), Id = 0 };
                var selectList = new List<DDItem>();
                if (hasDefault)
                {
                    selectList.Add(first);
                }
                var ls = await sysLookup.GetDataByCategory(categoryId, lang);
                if (ls.Succeeded)
                {
                    selectList.AddRange(ls.Data);
                }
                else
                {
                    if (hasDefault)
                        selectList[0].Name = "لا توجد بيانات";
                }
                return new SelectList(selectList, valueClm, textClm, selectedValue);
            }
            catch (Exception)
            {
                var error = new DDItem { Code = 0, Name = localization.GetCommonResource("Error! No Data Retrived"), Id = 0 };
                var selectList = new List<DDItem>();
                selectList.Add(error);
                return new SelectList(selectList, valueClm, textClm);
            }

        }
        private List<DDListItem<T>> ConvertToDDLItem<T, U>(IEnumerable<U> inputList, string textClm, string valueClm)
        {
            var ddlList = new List<DDListItem<T>>();

            foreach (var item in inputList)
            {
                var nameValue = item.GetType().GetProperty(textClm)?.GetValue(item, null);
                var valueValue = item.GetType().GetProperty(valueClm)?.GetValue(item, null);

                var ddlObject = new DDListItem<T>
                {
                    Name = nameValue != null ? nameValue.ToString() : string.Empty,
                    Value = valueValue != null ? (T)valueValue : default(T)
                };

                ddlList.Add(ddlObject);
            }

            return ddlList;
        }

        public async Task<DDLResult<TValue>> GetRawListPagination<TModel, TValue>(Expression<Func<TModel, bool>> where, string valueProperty, string nameProperty, TValue lastSeenId, int pageSize = 10) where TModel : class where TValue : IComparable<TValue>
        {
            try
            {
                // إنشاء parameter واحد فقط
                var parameter = Expression.Parameter(typeof(TModel), "x");

                var nameSelector = Expression.Lambda<Func<TModel, string>>(
                    Expression.PropertyOrField(parameter, nameProperty),
                    parameter
                );

                var valueSelector = Expression.Lambda<Func<TModel, TValue>>(
                    Expression.Convert(Expression.PropertyOrField(parameter, valueProperty), typeof(TValue)),
                    parameter
                );

                // ملاحظة: استدعاء الدالة بالشكل الصحيح حسب توقيع IDDLService.GetDDL
                var result = await ddlService.GetDDLPagination<TModel, TValue>(
                    where,
                    nameSelector,
                    valueSelector,
                    lastSeenId,
                    pageSize
                );
                return result;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return new DDLResult<TValue> { Items = new List<DDLItem<TValue>>() };
            }
        }
    }
}

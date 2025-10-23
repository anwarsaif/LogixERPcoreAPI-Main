using Logix.Application.Common;
using Logix.Application.Interfaces.IServices;
using Logix.MVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Logix.MVC.Reports
{
	public class CustomDevReportController : Controller
	{
		private readonly IMainServiceManager mainServiceManager;
		private readonly IMvcSession mvcSession;
		private readonly IDDListHelper listHelper;

		public CustomDevReportController(IMainServiceManager mainServiceManager,
			IDDListHelper listHelper,
            IMvcSession mvcSession)
		{
			this.mainServiceManager = mainServiceManager;
			this.listHelper = listHelper;
			this.mvcSession = mvcSession;
		}

		[HttpGet]
		public async Task<SelectList> GetUsersDdl() 
		{
            var allUsers = await mainServiceManager.SysUserService.GetAll(x => x.Isdel == false && x.IsDeleted == false
				&& x.Enable == 1 && x.FacilityId == mvcSession.FacilityId);
			var users = listHelper.GetFromList<long>(allUsers.Data.Select(s => new DDListItem<long> { Name = s.UserFullname ?? "", Value = s.Id }), hasDefault: false);
			return users;
		}

		[HttpGet]
		public async Task<SelectList> GetGroupsDdl()
		{
			var allGroups = await mainServiceManager.SysGroupService.GetAll(x => x.IsDeleted == false
				&& x.FacilityId == mvcSession.FacilityId);
			var groups = listHelper.GetFromList<int>(allGroups.Data.Select(s => new DDListItem<int> { Name = s.GroupName ?? "", Value = s.GroupId ?? 0 }), hasDefault: false);
			return groups;
		}
	}
}

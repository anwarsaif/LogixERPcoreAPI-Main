using Logix.Application.Common;
using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.Main;
using Logix.Application.Extensions;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Services;
using Logix.Domain.Main;
using Logix.MVC.ViewModels;
using System.Globalization;

namespace Logix.MVC.Helpers
{
    public interface IFilesHelper
    {
        Task<bool> SaveFiles(long primaryKey, long tableId, int fileType = 0);
        Task<bool> SaveFiles2(long primaryKey, long tableId, int fileType = 0, string? FileUrl = null,long CreatedBy= 0,string? ext=null);

        Task<bool> SaveCustomerFiles(long primaryKey, long tableId, int fileType = 0);
        Task<string> SaveFile(IFormFile file, string savePath = "AllFiles", int fileType = 0 );
        
        Task HandleFilesEdit(long primaryKey, long tableId);
        Task HandleFilesAdd(long primaryKey, long tableId);
        Task<bool> UpdateFiles(long primaryKey, long tableId, int fileType = 0);

    }

    public class FilesHelper : IFilesHelper
    {
        private readonly ISysFileService fileService;
        private readonly ISysCustomerFileService customerFileService;
        private readonly IWebHostEnvironment env;
        private readonly ISession _session;
        private readonly ICurrentData currentData;
        public FilesHelper(ISysFileService fileService, ISysCustomerFileService customerFileService, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, ICurrentData currentData)
        {
            this.fileService = fileService;
            this.customerFileService = customerFileService;
            this.env = env;
            this._session = httpContextAccessor.HttpContext.Session;
            this.currentData = currentData;
        }

        public async Task<string> SaveFile(IFormFile file, string savePath="AllFiles", int fileType = 0)
        {
            try
            {
                string fname = "";
                string unqName = "";
                string ext = "";
                var userId = currentData.UserId;
                if (file != null && file.Length > 0)
                {
                    string tempFileName = Path.GetTempFileName();
                    ext = Path.GetExtension(file.FileName);
                    string attFolder = Path.Combine(env.WebRootPath, savePath);
                    if (!Directory.Exists(attFolder))
                    {
                        Directory.CreateDirectory(attFolder);
                    }
                    unqName = String.Format("{0}{1}{2}",userId, DateTime.Now.ToString("ddmmyyhhmmssfff"),ext); //Guid.NewGuid().ToString().Substring(0, 10) + ext);

                    fname = Path.Combine(attFolder, unqName);
                    using (FileStream str = new FileStream(fname, FileMode.Create))
                    {
                        file.CopyTo(str);
                    }

                    //return Path.Combine(savePath,unqName);
                    return $"{savePath}/{unqName}";
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<bool> SaveFiles(long primaryKey, long tableId, int fileType = 0)
        {
            try
            {
                var files = _session.GetData<List<SysFileDto>>(SessionKeys.AddTempFiles);
                files = files.Where(f => f.TableId == tableId).ToList();
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string fname = "";
                        string unqName = "";
                        if (file != null)
                        {
                            string attFolder = Path.DirectorySeparatorChar + FilesPath.AllFiles;
                            unqName = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) + file.FileExt;
                            fname = Path.Join(attFolder, unqName);
                            
                            string permFilePath = Path.Join(env.WebRootPath, fname);
                            //System.IO.File.Copy(file.FileUrl??"", permFilePath, true);
                            if (file.IsFormScreen ==0)
                            {
                                System.IO.File.Copy(file.FileUrl??"", permFilePath, true);
                            }
                            if (System.IO.File.Exists(file.FileUrl))
                            {
                                System.IO.File.Delete(file.FileUrl);
                            }
                            file.Id = 0;
                            file.CreatedOn = DateTime.Now;
                            file.PrimaryKey = primaryKey;
                            file.TableId = int.TryParse(tableId.ToString(), out int result) ? result : 0;
                            file.FileType = fileType;
                            file.FileUrl = fname;
                            file.FileDescription =  file.FileDescription == null? "":file.FileDescription;

                            // save to database
                            var addRes = await fileService.Add(file);
                            
                        }
                    }

                    _session.AddData<List<SysFileDto>>(SessionKeys.AddTempFiles, new List<SysFileDto>());
                }
                else
                {
                    // ModelState.AddModelError(string.Empty, "=== There is no attachements founded ====");
                    
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> SaveFiles2(long primaryKey, long tableId, int fileType = 0, string? FileUrl=null,long CreatedBy=0, string ext = null)
        {
            try
            {
                var file = new SysFileDto();
               
                   
                        string fname = "";
                        string unqName = "";
                        if (file != null)
                        {
                            string attFolder = Path.DirectorySeparatorChar + FilesPath.AllFiles;
                            unqName = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) + file.FileExt;
                            fname = Path.Join(attFolder, unqName);

                            string permFilePath = Path.Join(env.WebRootPath, fname);
                            //System.IO.File.Copy(FileUrl ?? "", permFilePath, true);
                            if (System.IO.File.Exists(FileUrl))
                            {
                                System.IO.File.Delete(FileUrl);
                    }
                            file.FileName = "AttendanceInvoices";
                            file.CreatedOn = DateTime.Now;
                            file.PrimaryKey = primaryKey;
                            file.TableId = int.TryParse(tableId.ToString(), out int result) ? result : 0;
                            file.FileType = fileType;
                            file.FileUrl = FileUrl;
                    file.FileExt = ext;
                    file.CreatedBy = CreatedBy;
                            file.FileDescription =  "" ;
                    file.FileDate = DateTime.Now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

                    var addRes = await fileService.Add(file); 
                    if(addRes.Succeeded)
                    {
                        return true;
                    }
                        }
                    

               

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SaveCustomerFiles(long primaryKey, long tableId, int fileType = 0)
        {
            try
            {
                var files = _session.GetData<List<SysFileDto>>(SessionKeys.AddTempFiles);
                files = files.Where(f => f.TableId == tableId).ToList();
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string fname = "";
                        string unqName = "";
                        if (file != null)
                        {
                            string attFolder = Path.DirectorySeparatorChar + FilesPath.AllFiles;
                            unqName = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) + file.FileExt;
                            fname = Path.Join(attFolder, unqName);

                            string permFilePath = Path.Join(env.WebRootPath, fname);
                            System.IO.File.Copy(file.FileUrl ?? "", permFilePath, true);
                            if (System.IO.File.Exists(file.FileUrl))
                            {
                                System.IO.File.Delete(file.FileUrl);
                            }
                            file.CreatedOn = DateTime.Now;
                            file.PrimaryKey = primaryKey;
                            file.TableId = int.TryParse(tableId.ToString(), out int result) ? result : 0;
                            file.FileType = fileType;
                            file.FileUrl = fname;
                            file.FileDescription = file.FileDescription == null ? "" : file.FileDescription;
                            var addRes = await fileService.Add(file); // save to database
                        }
                    }

                    _session.AddData<List<SysFileDto>>(SessionKeys.AddTempFiles, new List<SysFileDto>());
                }
                else
                {
                    // ModelState.AddModelError(string.Empty, "=== There is no attachements founded ====");

                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task HandleFilesEdit(long Id, long TableId)
        {

            if (Id != 0)
            {
                var model = await fileService.GetAll(x => x.TableId == TableId && x.PrimaryKey == Id && x.IsDeleted == false);
                var Filess = _session.GetData<List<SysFileDto>>(SessionKeys.EditTempFiles);

                if (Filess == null)
                {
                    Filess = new List<SysFileDto>();
                }
                var No = 1;
                if (model != null)
                {
                    foreach (var obj in model.Data)
                    {


                        var temp = Filess.ToList();
                        if (temp != null)
                        {
                            Filess.Add(obj);
                            No += No;
                        }
                    }

                    _session.AddData<List<SysFileDto>>(SessionKeys.EditTempFiles, Filess);

                }
                else
                {
                }



            }
        }

        public async Task HandleFilesAdd(long Id, long TableId)
        {

            if (Id != 0)
            {
                var model = await fileService.GetAll(x => x.TableId == TableId && x.PrimaryKey == Id && x.IsDeleted == false);
                var Filess = _session.GetData<List<SysFileDto>>(SessionKeys.AddTempFiles);

                if (Filess == null)
                {
                    Filess = new List<SysFileDto>();
                }
                var No = 1;
                if (model != null)
                {
                    foreach (var obj in model.Data)
                    {

                        obj.IsFormScreen = 1;
                        var temp = Filess.ToList();
                        if (temp != null)
                        {
                            Filess.Add(obj);
                            No += No;
                        }
                    }

                    _session.AddData<List<SysFileDto>>(SessionKeys.AddTempFiles, Filess);

                }
                else
                {
                }



            }
        }
        public async Task<bool> UpdateFiles(long primaryKey, long tableId, int fileType = 0)
        {
            try
            {
                var files = _session.GetData<List<SysFileDto>>(SessionKeys.EditTempFiles);
                files = files.Where(f => f.TableId == tableId).ToList();
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string fname = "";
                        string unqName = "";
                        if (file != null)
                        {
                            if (file.IsDeleted == false && file.Id == 0)
                            {
                               

                            
                                //string fileName = Path.GetFileNameWithoutExtension(file.Url);
                                //  string attFolder = Path.Combine("", FilesPath.AllFiles);
                                string attFolder = Path.DirectorySeparatorChar + FilesPath.AllFiles;
                            //unqName = Guid.NewGuid().ToString().Substring(0, 18) + file.FileExt;
                            unqName = DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture) + file.FileExt;
                            fname = Path.Join(attFolder, unqName);

                            string permFilePath = Path.Join(env.WebRootPath, fname);
                            System.IO.File.Copy(file.FileUrl ?? "", permFilePath, true);
                            if (System.IO.File.Exists(file.FileUrl))
                            {
                                System.IO.File.Delete(file.FileUrl);
                            }
                            file.CreatedOn = DateTime.Now;
                            file.PrimaryKey = primaryKey;
                            file.TableId = int.TryParse(tableId.ToString(), out int result) ? result : 0;
                            file.FileType = fileType;
                            file.FileUrl = fname;
                            file.FileDescription = file.FileDescription == null ? "" : file.FileDescription;
                                var addRes = await fileService.Add(file); // save to database
                            }
                            else
                            {
                                var addRes = await fileService.Update(file); // save to database

                            }


                        }
                    }

                    _session.AddData<List<SysFileDto>>(SessionKeys.EditTempFiles, new List<SysFileDto>());
                }
                else
                {
                    // ModelState.AddModelError(string.Empty, "=== There is no attachements founded ====");

                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}

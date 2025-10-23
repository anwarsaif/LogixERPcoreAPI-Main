using Microsoft.Extensions.Logging;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Globalization;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysNotificationsMangRepository : GenericRepository<SysNotificationsMang>, ISysNotificationsMangRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentData session;
        private readonly ILogger<SysNotificationsMangRepository> logger;
        private readonly IConfiguration configuration;
        public SysNotificationsMangRepository(ApplicationDbContext context, ICurrentData session, ILogger<SysNotificationsMangRepository> _logger, IConfiguration configuration) : base(context)
        {
            this.context = context;
            this.session = session;
            this.logger = _logger;
            this.configuration = configuration;
        }

        //public async Task<List<SysNotificationsMangResultDto>> GetNotificationsByUserAndGroupAsync()
        //{
        //    long userId = session.UserId;
        //    long groupId = session.GroupId;
        //    DateTime currentDate = DateHelper.GetCurrentDateTime();
        //    var resultTable = new List<SysNotificationsMangResultDto>();

        //    try
        //    {
        //        string fetchNotificationsQuery = @"
        //SELECT * 
        //FROM Sys_Notifications_Mang_VW
        //WHERE IsDeleted = 0
        //AND (@UserId IN (SELECT value FROM dbo.fn_Split(USER_ID, ',')) 
        //    OR @GroupId IN (SELECT value FROM dbo.fn_Split(Group_ID, ',')))";

        //        using (var connection = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
        //        {
        //            await connection.OpenAsync();

        //            // Execute the fetchNotificationsQuery
        //            using (var command = new SqlCommand(fetchNotificationsQuery, connection))
        //            {
        //                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = userId });
        //                command.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.BigInt) { Value = groupId });

        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    var notifications = new List<SysNotificationsMangVw>();
        //                    while (await reader.ReadAsync())
        //                    {
        //                        notifications.Add(new SysNotificationsMangVw
        //                        {
        //                            Id = reader.GetInt64(reader.GetOrdinal("ID")),
        //                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
        //                            UserId = reader.IsDBNull(reader.GetOrdinal("User_ID")) ? null : reader.GetString(reader.GetOrdinal("User_ID")),
        //                            TableId = reader.IsDBNull(reader.GetOrdinal("Table_ID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("Table_ID")),
        //                            SelectFieldId = reader.IsDBNull(reader.GetOrdinal("Select_Field_ID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("Select_Field_ID")),
        //                            ConditionFieldId = reader.IsDBNull(reader.GetOrdinal("Condition_Field_ID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("Condition_Field_ID")),
        //                            AheadOf = reader.IsDBNull(reader.GetOrdinal("Ahead_Of")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("Ahead_Of")),
        //                            ConditionOthers = reader.IsDBNull(reader.GetOrdinal("Condition_Others")) ? null : reader.GetString(reader.GetOrdinal("Condition_Others")),
        //                            IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
        //                            TableDescription = reader.IsDBNull(reader.GetOrdinal("Table_Description")) ? null : reader.GetString(reader.GetOrdinal("Table_Description")),
        //                            Desc1 = reader.IsDBNull(reader.GetOrdinal("Desc_1")) ? null : reader.GetString(reader.GetOrdinal("Desc_1")),
        //                            TableName = reader.IsDBNull(reader.GetOrdinal("Table_Name")) ? null : reader.GetString(reader.GetOrdinal("Table_Name")),
        //                            SelectFieldName = reader.IsDBNull(reader.GetOrdinal("Select_Field_Name")) ? null : reader.GetString(reader.GetOrdinal("Select_Field_Name")),
        //                            ConditionFieldName = reader.IsDBNull(reader.GetOrdinal("Condition_Field_Name")) ? null : reader.GetString(reader.GetOrdinal("Condition_Field_Name")),
        //                            Desc12 = reader.IsDBNull(reader.GetOrdinal("Desc_12")) ? null : reader.GetString(reader.GetOrdinal("Desc_12")),
        //                            AssigneeTypeId = reader.IsDBNull(reader.GetOrdinal("Assignee_Type_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Assignee_Type_ID")),
        //                            GroupId = reader.IsDBNull(reader.GetOrdinal("Group_ID")) ? null : reader.GetString(reader.GetOrdinal("Group_ID"))
        //                        });
        //                    }

        //                    foreach (var notification in notifications)
        //                    {
        //                        try
        //                        {
        //                            var query = $@"
        //                    SELECT 
        //                        N'{notification.Name}' AS Name,
        //                        {notification.SelectFieldName} AS Select_Field_Name,
        //                        {notification.ConditionFieldName} AS Condition_Field_Name
        //                    FROM 
        //                        {notification.TableName}
        //                    WHERE 
        //                        {notification.ConditionFieldName} IS NOT NULL
        //                        AND {notification.ConditionFieldName} <> ''
        //                        AND dbo.DateDiff_day({notification.ConditionFieldName}, @CurrDate) >= -{notification.AheadOf}
        //                        {notification.ConditionOthers}";

        //                            using (var notificationCommand = connection.CreateCommand())
        //                            {
        //                                notificationCommand.CommandText = query;
        //                                notificationCommand.Parameters.Add(new SqlParameter("@CurrDate", currentDate));
        //                                notificationCommand.Parameters.Add(new SqlParameter("@Emp_ID", session.EmpId));

        //                                using (var notificationReader = await notificationCommand.ExecuteReaderAsync())
        //                                {
        //                                    if (notificationReader.HasRows)
        //                                    {
        //                                        while (await notificationReader.ReadAsync())
        //                                        {
        //                                            resultTable.Add(new SysNotificationsMangResultDto
        //                                            {
        //                                                Name = notificationReader["Name"].ToString(),
        //                                                SelectFieldName = notificationReader["Select_Field_Name"].ToString(),
        //                                                ConditionFieldName = notificationReader["Condition_Field_Name"].ToString()
        //                                            });
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            logger.LogError(ex, $"Error executing query for notification: {notification.Name}");
        //                            continue;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Error fetching notifications from SysNotificationsMangVw");
        //    }

        //    return resultTable;
        //}
        public async Task<List<SysNotificationsMangResultDto>> GetNotificationsByUserAndGroupAsync()
        {
            long userId = session.UserId;
            long groupId = session.GroupId;
            string currentDate = DateHelper.GetCurrentDateTime().ToString("yyyy/MM/dd",CultureInfo.InvariantCulture);
            var resultTable = new List<SysNotificationsMangResultDto>();

            try
            {
                string fetchNotificationsQuery = @"
        SELECT * 
        FROM Sys_Notifications_Mang_VW
        WHERE IsDeleted = 0
        AND (@UserId IN (SELECT value FROM dbo.fn_Split(USER_ID, ',')) 
            OR @GroupId IN (SELECT value FROM dbo.fn_Split(Group_ID, ',')))";

                string connectionString = configuration.GetConnectionString("LogixLocal") + ";MultipleActiveResultSets=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Execute the fetchNotificationsQuery
                    using (var command = new SqlCommand(fetchNotificationsQuery, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = userId });
                        command.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.BigInt) { Value = groupId });

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var notifications = new List<SysNotificationsMangVw>();
                            while (await reader.ReadAsync())
                            {
                                notifications.Add(new SysNotificationsMangVw
                                {
                                    Id = reader.GetInt64(reader.GetOrdinal("ID")),
                                    Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                                    UserId = reader.IsDBNull(reader.GetOrdinal("User_ID")) ? null : reader.GetString(reader.GetOrdinal("User_ID")),
                                    TableId = reader.IsDBNull(reader.GetOrdinal("Table_ID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("Table_ID")),
                                    SelectFieldId = reader.IsDBNull(reader.GetOrdinal("Select_Field_ID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("Select_Field_ID")),
                                    ConditionFieldId = reader.IsDBNull(reader.GetOrdinal("Condition_Field_ID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("Condition_Field_ID")),
                                    AheadOf = reader.IsDBNull(reader.GetOrdinal("Ahead_Of")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("Ahead_Of")),
                                    ConditionOthers = reader.IsDBNull(reader.GetOrdinal("Condition_Others")) ? null : reader.GetString(reader.GetOrdinal("Condition_Others")),
                                    IsDeleted = reader.IsDBNull(reader.GetOrdinal("IsDeleted")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                                    TableDescription = reader.IsDBNull(reader.GetOrdinal("Table_Description")) ? null : reader.GetString(reader.GetOrdinal("Table_Description")),
                                    Desc1 = reader.IsDBNull(reader.GetOrdinal("Desc_1")) ? null : reader.GetString(reader.GetOrdinal("Desc_1")),
                                    TableName = reader.IsDBNull(reader.GetOrdinal("Table_Name")) ? null : reader.GetString(reader.GetOrdinal("Table_Name")),
                                    SelectFieldName = reader.IsDBNull(reader.GetOrdinal("Select_Field_Name")) ? null : reader.GetString(reader.GetOrdinal("Select_Field_Name")),
                                    ConditionFieldName = reader.IsDBNull(reader.GetOrdinal("Condition_Field_Name")) ? null : reader.GetString(reader.GetOrdinal("Condition_Field_Name")),
                                    Desc12 = reader.IsDBNull(reader.GetOrdinal("Desc_12")) ? null : reader.GetString(reader.GetOrdinal("Desc_12")),
                                    AssigneeTypeId = reader.IsDBNull(reader.GetOrdinal("Assignee_Type_ID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Assignee_Type_ID")),
                                    GroupId = reader.IsDBNull(reader.GetOrdinal("Group_ID")) ? null : reader.GetString(reader.GetOrdinal("Group_ID"))
                                });
                            }

                            foreach (var notification in notifications)
                            {
                                try
                                {
                                    var query = $@"
                            SELECT 
                                N'{notification.Name}' AS Name,
                                {notification.SelectFieldName} AS Select_Field_Name,
                                {notification.ConditionFieldName} AS Condition_Field_Name
                            FROM 
                                {notification.TableName}
                            WHERE 
                                {notification.ConditionFieldName} IS NOT NULL
                                AND {notification.ConditionFieldName} <> ''
                                AND dbo.DateDiff_day({notification.ConditionFieldName}, @CurrDate) >= -{notification.AheadOf}
                                {notification.ConditionOthers}";

                                    using (var notificationCommand = connection.CreateCommand())
                                    {
                                        notificationCommand.CommandText = query;
                                        notificationCommand.Parameters.Add(new SqlParameter("@CurrDate", currentDate));
                                        notificationCommand.Parameters.Add(new SqlParameter("@Emp_ID", session.EmpId));

                                        using (var notificationReader = await notificationCommand.ExecuteReaderAsync())
                                        {
                                            if (notificationReader.HasRows)
                                            {
                                                while (await notificationReader.ReadAsync())
                                                {
                                                    resultTable.Add(new SysNotificationsMangResultDto
                                                    {
                                                        Name = notificationReader["Name"].ToString(),
                                                        SelectFieldName = notificationReader["Select_Field_Name"].ToString(),
                                                        ConditionFieldName = notificationReader["Condition_Field_Name"].ToString()
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(ex, $"Error executing query for notification: {notification.Name}");
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching notifications from SysNotificationsMangVw");
            }

            return resultTable;
        }

    }
}

using Autofac.Core;
using AutoMapper;
using Microsoft.Build.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Logix.MVC.Helpers
{
    public static class ExceptionsHelper
    {
        public static string GetCustomErrorMessage(Exception exception)
        {
            switch (exception)
            {
                case ArgumentNullException _:
                    return "One or more arguments are null.";
                case InvalidOperationException _:
                    return "An invalid operation occurred.";
                case NotSupportedException _:
                    return "The operation is not supported.";
                case ArgumentException _:
                    return "An argument is not valid.";
                case HttpRequestException _:
                    return "An error occurred while making an HTTP request.";
                case FileNotFoundException _:
                    return "The requested file could not be found.";
                case SqlException _:
                    return "An error occurred while working with the SQL database.";
                case DbUpdateException _:
                    return "An error occurred while updating the database.";
                case ValidationException _:
                    return "A validation error occurred.";

                case UnauthorizedAccessException _:
                    return "Access to the resource is denied.";

                case CircularDependencyException _:
                    return "It occurs when two or more services depend on each other, creating an infinite loop in the dependency resolution process";

                case AutoMapperConfigurationException _:
                    return "It typically occurs when there is a mapping configuration problem, such as missing or incorrect mappings between types";

                case AutoMapperMappingException _:
                    return "It can happen if there is an issue with the source or destination types, or if a required mapping is missing.";

                case DependencyResolutionException _:
                    return "It can occur if there is a missing or misconfigured registration, or if there are conflicts between registered services.";

                case DBConcurrencyException _:
                    return "It happens when another user or process has modified the same data that is being saved or updated concurrently.";

                case FormatException _:
                    return "This is thrown when a format of an argument or input is invalid for the specified operation.";



                default:
                    return "An exception occurred.";
            }
        }
    }
}

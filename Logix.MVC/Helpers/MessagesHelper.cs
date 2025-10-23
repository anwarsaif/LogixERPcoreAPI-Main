using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace Logix.MVC.Helpers
{
    public static class MessagesHelper
    {
        public static bool AddErrorMessage(this ITempDataDictionary tempData, string messageKey)
        {
            try
            {
                return tempData.SetTemp<string>(key: "LogixErrorMessage", value: messageKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------- Exception in SetTemp -----------");
                Console.WriteLine(ex.ToString());
                return false;
            }

        }
        public static bool AddSuccessMessage(this ITempDataDictionary tempData, string messageKey)
        {
            try
            {
                return tempData.SetTemp<string>(key: "LogixSuccessMessage", value: messageKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------- Exception in SetTemp -----------");
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public static bool AddWarningMessage(this ITempDataDictionary tempData, string messageKey)
        {
            try
            {
                return tempData.SetTemp<string>(key: "LogixWarningMessage", value: messageKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------- Exception in SetTemp -----------");
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public static bool AddInfoMessage(this ITempDataDictionary tempData, string messageKey)
        {
            try
            {
                return tempData.SetTemp<string>(key: "LogixInfoMessage", value: messageKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------- Exception in SetTemp -----------");
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public static string? GetErrorMessage(this ITempDataDictionary tempData)
        {
            try
            {
                return tempData.GetTemp<string>(key: "LogixErrorMessage");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string? GetSuccessMessage(this ITempDataDictionary tempData)
        {
            try
            {
                return tempData.GetTemp<string>(key: "LogixSuccessMessage");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string? GetInfoMessage(this ITempDataDictionary tempData)
        {
            try
            {
                return tempData.GetTemp<string>(key: "LogixInfoMessage");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string? GetWarningMessage(this ITempDataDictionary tempData)
        {
            try
            {
                return tempData.GetTemp<string>(key: "LogixWarningMessage");
            }
            catch (Exception)
            {
                return null;
            }
        }


        // ==================== internal functions ====================
        private static bool SetTemp<T>(this ITempDataDictionary tempData, string key, T value)
        {
            try
            {
                tempData[key] = JsonSerializer.Serialize(value);
                if (tempData.ContainsKey(key))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------- Exception in SetTemp -----------");
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        private static T? GetTemp<T>(this ITempDataDictionary tempData, string key)
        {
            T? value;
            try
            {
                if (tempData.ContainsKey(key))
                {
                    object ob;
                    bool res = tempData.TryGetValue(key, out ob);
                    if (res)
                    {
                        if (ob != null)
                        {
                            value = JsonSerializer.Deserialize<T>((string)ob);
                            if (value != null)
                            {
                                return value;
                            }
                        }
                        else
                        {
                            return default;
                        }
                    }
                    else
                    {
                        return default;
                    }

                }
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------- Exception in GetSave ---------------");
                Console.WriteLine(ex.ToString());
                return default;
            }
        }


        private static T? GetTempSave<T>(this ITempDataDictionary tempData, string key)
        {
            T? value;
            try
            {
                if (tempData.ContainsKey(key))
                {
                    object? ob;
                    ob = tempData.Peek(key);
                    if (ob != null)
                    {
                        value = JsonSerializer.Deserialize<T>((string)ob);
                        if (value != null)
                        {
                            return value;
                        }
                        else
                        {
                            return default;
                        }
                    }
                    else
                    {
                        return default;
                    }

                }
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("------------- Exception in GetSave ---------------");
                Console.WriteLine(ex.ToString());
                return default;
            }
        }
    }
}

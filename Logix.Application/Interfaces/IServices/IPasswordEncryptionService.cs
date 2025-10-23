using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface IPasswordEncryptionService
    {
        byte[] EncryptPassword(string data);
        string DecryptPassword(byte[] encryptedData);
    }
}

using System;
namespace ATS.Service
{
    public interface IAuthService
    {
        ATS.Core.Domain.DomainModels.UserAccount Authenticate(string email, string password);

        //Int64 GetCustomerIDByAccountID(Int64 accountID);
        //Int64 GetEmployeeIDByAccountID(Int64 accountID);
        
    }
}

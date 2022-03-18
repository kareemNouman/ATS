using ATS.Core.Domain.DomainModels;
using ATS.Core.Domain.ResponseModels;
using ATS.Data;
using ATS.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Service
{
    /// <summary>
    /// services for user specific operations
    /// </summary>
    public class AuthService : ATS.Service.IAuthService
    {
        private readonly IGenericRepository<UserAccount> _userRepository;       
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWrk;
        /// <summary>
        /// Public constructor.
        /// </summary>
        public AuthService(IGenericRepository<UserAccount> userRepository, IEncryptionService encryptionService, IUnitOfWork unitOfWrk)
        {
            this._userRepository = userRepository;          
            this._encryptionService = encryptionService;
            this._unitOfWrk = unitOfWrk;
        }


        /// <summary>
        /// Public method to authenticate user by email and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>UserAccount</returns>
        public UserAccount Authenticate(string email, string password)
        {
            try
            {
                var account = _userRepository.FirstOrDefault(u => u.UserName == email);
                if (account == null)
                    return null;

                var pwd = _encryptionService.CreatePasswordHash(password, account.Salt);
                bool isValid = pwd == account.PasswordHash;


                //save last login date
                if (isValid)
                {
                    //var lastLogin = account.LastLogin;
                    //account.LastLogin = DateTime.UtcNow;
                    //_unitOfWrk.Save();
                    //account.LastLogin = lastLogin;
                    return account;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }

        ///// <summary>
        ///// Public method to Get CustomerID by Account ID
        ///// </summary>
        ///// <param name="email"></param>
        ///// <param name="password"></param>
        ///// <returns>CustomerID</returns>
        //public Int64 GetCustomerIDByAccountID(Int64 accountID)
        //{
        //    return _customerRepository.GetWithInclude(x => x.AccountID == accountID).Select(x => x.ID).FirstOrDefault();
        //}

        ///// <summary>
        ///// Public method to Get EmployeeID by Account ID
        ///// </summary>
        ///// <param name="email"></param>
        ///// <param name="password"></param>
        ///// <returns>EmployeeID</returns>
        //public Int64 GetEmployeeIDByAccountID(Int64 accountID)
        //{
        //    return _employeeRepository.GetWithInclude(x => x.AccountID == accountID).Select(x => x.ID).FirstOrDefault();
        //}


    }


    public class BaseService
    {

        /// <summary>
        /// Creates a paged set of results.
        /// </summary>
        /// <typeparam name="T">The type of the source IQueryable.</typeparam>
        /// <typeparam name="TReturn">The type of the returned paged results.</typeparam>
        /// <param name="queryable">The source IQueryable.</param>
        /// <param name="page">The page number you want to retrieve.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="orderBy">The field or property to order by.</param>
        /// <param name="ascending">Indicates whether or not 
        /// the order should be ascending (true) or descending (false.)</param>
        /// <returns>Returns a paged set of results.</returns>
        protected PagedResults<TResult> CreatePagedResults<T, TResult>(
            IQueryable<T> queryable,
            int? page,
            int? pageSize,
            string orderBy,
            bool ascending,
            Func<T, TResult> action)
        {

            page = page ?? 1;
            pageSize = pageSize ?? 10;

            var skipAmount = pageSize.Value * (page.Value - 1);

            var projection = queryable
                .OrderByPropertyOrField(orderBy, ascending)
                .Skip(skipAmount)
                .Take(pageSize.Value)
                .Select(action);

            var totalNumberOfRecords = queryable.Count();
            var results = projection.ToList();

            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = (totalNumberOfRecords / pageSize.Value) + (mod == 0 ? 0 : 1);

            //var nextPageUrl =
            //page == totalPageCount
            //    ? null
            //    : Url?.Link("DefaultApi", new
            //    {
            //        page = page + 1,
            //        pageSize,
            //        orderBy,
            //        ascending
            //    });

            return new PagedResults<TResult>
            {
                Results = results,
                PageNumber = page.Value,
                PageSize = results.Count,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords,
                //NextPageUrl = nextPageUrl
            };
        }



        protected PagedResults<TResult> PagedResults<T, TResult>(
           IQueryable<T> queryable,
           int? skip,
           int? pageSize,
           string orderBy,
           bool ascending,
           Func<T, TResult> action)
        {

            skip = skip ?? 0;
            pageSize = pageSize ?? 10;

            //var projection = queryable
            //    .OrderByPropertyOrField(orderBy, ascending)
            //    .Skip(skip.Value)
            //    .Take(pageSize.Value)
            //    .Select(action);


            var results = queryable
                .OrderByPropertyOrField(orderBy, ascending)
                .Skip(skip.Value)
                .Take(pageSize.Value)
                .ToList();

            var res = results.Select(action).ToList();

            var totalNumberOfRecords = queryable.Count();

            return new PagedResults<TResult>
            {
                Results = res,
                PageSize = results.Count,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }

    }


    public static class Extensions
    {
        /// <summary>
        /// Order the IQueryable by the given property or field.
        /// </summary>

        /// <typeparam name="T">The type of the IQueryable being ordered.</typeparam>
        /// <param name="queryable">The IQueryable being ordered.</param>
        /// <param name="propertyOrFieldName">
        /// The name of the property or field to order by.</param>
        /// <param name="ascending">Indicates whether or not 
        /// the order should be ascending (true) or descending (false.)</param>
        /// <returns>Returns an IQueryable ordered by the specified field.</returns>
        public static IQueryable<T> OrderByPropertyOrField<T>
        (this IQueryable<T> queryable, string propertyOrFieldName, bool ascending = true)
        {
            var elementType = typeof(T);
            var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

            var parameterExpression = Expression.Parameter(elementType);
            var propertyOrFieldExpression =
                Expression.PropertyOrField(parameterExpression, propertyOrFieldName);
            var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

            var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName,
                new[] { elementType, propertyOrFieldExpression.Type }, queryable.Expression, selector);

            return queryable.Provider.CreateQuery<T>(orderByExpression);
        }
    }
}

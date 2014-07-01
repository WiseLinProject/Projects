using Oleit.AS.Service.DataObject;
using Oleit.AS.Service.LogicService.UserAccessReference;
using Oleit.AS.Service.LogicService.EntityAccessReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Data;
//using Newtonsoft.Json;



namespace Oleit.AS.Service.LogicService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InternalUserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select InternalUserService.svc or InternalUserService.svc.cs at the Solution Explorer and start debugging.
    public class InternalUserService : IInternalUserService
    {
       
        public void NewUser(User user)
        {
            using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            {
                _userAccessClient.Insertuser(user);
            }
        }

        public void NewUser(UserCollection collection)
        {
            using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            {
                _userAccessClient.InsertuserCollection(collection.ToArray());
            }
        }

        public int CheckPassword(string userName, string password)
        {
            PrincipalContext adContext = new PrincipalContext(ContextType.Domain);
            if(adContext.ValidateCredentials(userName, password))
            {
                using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
                {
                    var _user=  _userAccessClient.QueryuserName(userName);
                    if(_user.Any()&& (_user.FirstOrDefault().Enable==1))
                        return _user[0].UserID;
                    else
                        return -1;
                }
            }
            else
            {
                return -1;
            }



            //if (IsInternalUser.Equals("Y"))
            //{
            //if (adContext.ValidateCredentials(userName, password))
            //{
            //    UserPrincipal user = UserPrincipal.Current;
            //    return user.DisplayName;
            //}
            //else
            //    return "False";
            //}
            //else
            //{
            //    using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            //    {
            //        if (_userAccessClient.CheckPassword(userName, password))
            //            return "True";
            //        else
            //            return "False";
            //    }                
            //}
        }

        public UserCollection QueryAlluser()
        {
            using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            {
                return new UserCollection(_userAccessClient.QueryAll());
            }
        }
        public UserCollection QueryADuser()
        {
            using (PrincipalContext _adContext = new PrincipalContext(ContextType.Domain,"oleit.com.tw"))
            {
                try
                {
                    UserPrincipal _adUser = new UserPrincipal(_adContext);
                    PrincipalSearcher _srch = new PrincipalSearcher(_adUser);

                    // List<string> _ADUser = new List<string>();

                    //Query user table
                    UserCollection _collection = QueryAlluser();
                    UserCollection _ADcollection = new UserCollection();

                    foreach (var found in _srch.FindAll())
                    {
                        UserPrincipal _userp = found as UserPrincipal;
                        User _aduser = new User();
                        if (_userp != null)
                        {
                            if (_userp.DisplayName != null)
                            {
                                bool _isexit = false;
                                foreach (User user in _collection)
                                {
                                    if (_userp.SamAccountName.Equals(user.UserName))
                                        _isexit = true;
                                }
                                if (!_isexit)
                                {
                                    _aduser.UserName = _userp.SamAccountName;
                                    _ADcollection.Add(_aduser);
                                }
                                //_ADUser.Add(_userp.DisplayName + "," + _userp.SamAccountName);
                            }
                        }
                    }
                    return _ADcollection;
                }
                catch (PrincipalServerDownException)
                {
                    System.Threading.Thread.Sleep(3000);
                    return QueryADuser();
                }
                catch
                {
                    System.Threading.Thread.Sleep(3000);
                    return QueryADuser();
                }
            }
           

            //DataTable dt = new DataTable();
            //dt.Columns.Add("Name");
            //dt.Columns.Add("Sid");
            //dt.AcceptChanges();
            //foreach (var found in srch.FindAll())
            //{
            //    UserPrincipal user = found as UserPrincipal;
            //    if (user != null)
            //    {
            //        if (user.DisplayName != null)
            //        {                      
            //            dt.Rows.Add(new[] { user.DisplayName, user.SamAccountName });                      
            //        }
            //    }
            //}
            //return JsonConvert.SerializeObject(dt, Formatting.Indented);           
        }
        
           
        public void SetRole(string userName, Role role)
        {
            using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            {
                _userAccessClient.UpdateRoles(userName, role);
            }
        }

        public bool CheckRole(string userName, Role role)
        {
            using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            {
                return _userAccessClient.CheckRole(userName, role);
            }
        }

        public RoleCollection GetAllRoles(string userName)
        {
            using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            {
                return new RoleCollection(_userAccessClient.GetAllRoles(userName));
            }
        }

        public bool DisableUser(int userID)
        {
            using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            {
                return _userAccessClient.DisableUser(userID);
            }
        }

        public void Disable(UserCollection collection)
        {
            using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            {
                _userAccessClient.Disable(collection.ToArray());
            }
        }

        public void SetRelateEntity(Relation relation)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                _entityAccessClient.SetRelateEntity(relation);
            }
        }

        public RelationCollection GetRelateEntity(int entityID)
        {
            using (EntityAccessClient _entityAccessClient = new EntityAccessClient(EndpointName.EntityAccess))
            {
                return new RelationCollection(_entityAccessClient.GetRelateEntity(entityID));
            }
        }

        public FuncMenuCollection GetMenu(string userName)
        {
            using (UserAccessClient _userAccessClient = new UserAccessClient(EndpointName.UserAccess))
            {
                return new FuncMenuCollection(_userAccessClient.GetMenu(userName));
            }
        }
    }
}

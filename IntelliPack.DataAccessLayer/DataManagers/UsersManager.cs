using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliPack.DataAccessLayer.Models;
using System.Data.SqlClient;
using IntelliPack.DataAccessLayer.Base;

namespace IntelliPack.DataAccessLayer.DataManagers
{
    public class UsersManager : BaseManager<Users>
    {
        public UsersManager()
            : base()
        {

        }
        
            public List<Users> GetAdmins()
        {
            var result = Get("GET_Admins");
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Users> GetUsers()
        {
            var result = Get("GET_Users");
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Users> GetCouriers()
        {
            var result = Get("GET_Couriers");
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        
        public List<Users> GetUsersCourier()
        {
            var result = Get("GET_Users_Courier");
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public List<Users> GetUsersByEmail(string email)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@email", email)};
            var result = Get("GET_Users_By_Email @email", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                return result;
            }
        }
        public Users GetAuthentication(string username, string password)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@username", username),
            new SqlParameter("@password", password)};
            var result = Get("GET_Users_By_Auth @username, @password", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                Users cargo = new Users();
                if (result.Count > 0)
                {
                    cargo = result[0];
                }
                return cargo;
            }
        }
        public Users GetUsers(int user_id)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@userid", user_id)};
            var result = Get("GET_Users_By_ID @userid", parameters);
            if (result == null || !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
            else
            {
                Users cargo = new Users();
                if (result.Count > 0)
                {
                    cargo = result[0];
                }
                return cargo;
            }
        }
        public void Update(Users model)
        {
            var parameters = new SqlParameter[]{ 
                    new SqlParameter("@usersid", model.usersId),
                    new SqlParameter("@name", model.name),
                    new SqlParameter("@last_name", model.last_name),
                    new SqlParameter("@email", model.email),
                    new SqlParameter("@ID", model.ID),
                    new SqlParameter("@username", model.username),
                    new SqlParameter("@passwords", model.passwords),
                    new SqlParameter("@date_of_birth", model.date_of_birth),
                    new SqlParameter("@addresss", model.addresss),
                    new SqlParameter("@city_code", model.city_code),
                    new SqlParameter("@package_address", model.package_address),
                     new SqlParameter("@courierid", model.CourierId),
                     new SqlParameter("@phone_no", model.Phone_No),
                    new SqlParameter("@segundo_nombre", model.Segundo_nombre),
                    new SqlParameter("@segundo_apellido", model.Segundo_apellido)


            };
            var result = Get(@"UPDATE_SIMPLE_USER @usersid, @name,@last_name ,@email,@ID,@username,@passwords,@date_of_birth,@addresss,@city_code,@package_address, @courierid,@phone_no, @segundo_nombre, @segundo_apellido", parameters);

            if (result != null && result.Count > 0 && result[0].ErrorMessage != null && result[0].ErrorMessage.Trim() != "")
            {
                Error_Message = result[0].ErrorMessage;
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
        public void Set(Users model)
        {
               var parameters = new SqlParameter[]{
                    new SqlParameter("@name", model.name),
                    new SqlParameter("@last_name", model.last_name),
                    new SqlParameter("@email", model.email),
                    new SqlParameter("@ID", model.ID),
                    new SqlParameter("@username", model.username),
                    new SqlParameter("@passwords", model.passwords),
                    new SqlParameter("@date_of_birth", model.date_of_birth),
                    new SqlParameter("@addresss", model.addresss),
                    new SqlParameter("@city_code", model.city_code),
                    new SqlParameter("@courierid", model.CourierId),
                    new SqlParameter("@phone_no", model.Phone_No),
                    new SqlParameter("@segundo_nombre", model.Segundo_nombre),
                    new SqlParameter("@segundo_apellido", model.Segundo_apellido)

            };
            var result = Get(@"INSERT_SIMPLE_USER @name,@last_name ,@email,@ID,@username,@passwords,@date_of_birth,@addresss,@city_code,@courierid,@phone_no, @segundo_nombre, @segundo_apellido", parameters);

            if (result != null && result.Count > 0 && result[0].ErrorMessage != null && result[0].ErrorMessage.Trim() != "")
            {
                Error_Message = result[0].ErrorMessage;
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }

        public void SetCourier(Users model)
        {
            var parameters = new SqlParameter[]{
                    new SqlParameter("@name", model.name),
                    new SqlParameter("@last_name", model.last_name),
                    new SqlParameter("@email", model.email),
                    new SqlParameter("@ID", model.ID),
                    new SqlParameter("@username", model.username),
                    new SqlParameter("@passwords", model.passwords),
                    new SqlParameter("@date_of_birth", model.date_of_birth),
                    new SqlParameter("@addresss", model.addresss),
                    new SqlParameter("@city_code", model.city_code),
                    new SqlParameter("@courierid", model.CourierId),
                    new SqlParameter("@phone_no", model.Phone_No),
                    new SqlParameter("@segundo_nombre", model.Segundo_nombre),
                    new SqlParameter("@segundo_apellido", model.Segundo_apellido),
                    new SqlParameter("@lat", model.lat),
                    new SqlParameter("@lng", model.lng)

            };
            var result = Get(@"INSERT_COURIER_USER @name,@last_name ,@email,@ID,@username,@passwords,@date_of_birth,@addresss,@city_code,@courierid,@phone_no, @segundo_nombre, @segundo_apellido,@lat,@lng", parameters);

            if (result != null && result.Count > 0 && result[0].ErrorMessage != null && result[0].ErrorMessage.Trim() != "")
            {
                Error_Message = result[0].ErrorMessage;
            }
            if (Error_Message != null && !string.IsNullOrEmpty(Error_Message))
            {
                throw new Exception(Error_Message);
            }
        }
    }
}

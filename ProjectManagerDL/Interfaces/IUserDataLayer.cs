using ProjectManagerEntity;
using System.Collections.Generic;

namespace ProjectManagerDL
{
    public interface IUserDataLayer
    {
        List<UserEntity> GetAllUsers();
        void AddUser(UserEntity user);
        void UpdateUser(UserEntity user);
        void DeleteUser(int employeeId);
    }
}

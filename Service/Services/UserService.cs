﻿using DataAccess.Models;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Service.Enums;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }


        // public async Task<IEnumerable<User>> GetListUserByRole(AppEnums.ROLE_ENUMS role)
        // {
        //     int roleId = (int)role;

        //     return await unitOfWork.Users.GetAll(u => u.RoleId == roleId, u => u.Include(u => u.Role));
        // }

        // Create (Add) a new User
        public async Task<User> AddUser(User user)
        {
            try
            {
                return await _unitOfWork.Users.Add(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the user: {ex.Message}");
                throw new Exception("Could not add the user.", ex);
            }
        }

        // Read (Get) a User by ID
        public async Task<User?> GetUserById(int userId)
        {
            try
            {
                return await _unitOfWork.Users.GetFirstOrDefault(u => u.Id == userId, q => q.Include(u => u.Role));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the user: {ex.Message}");
                throw new Exception("Could not retrieve the user.", ex);
            }
        }

        // Read (Get) a List of Users by Role
        public async Task<IEnumerable<User>> GetListUserByRole(AppEnums.ROLE_ENUMS role)
        {
            int roleId = (int)role;

            try
            {
                return await _unitOfWork.Users.GetAll(u => u.RoleId == roleId, q => q.Include(u => u.Role));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving users by role: {ex.Message}");
                throw new Exception("Could not retrieve users by role.", ex);
            }
        }

        // Update an existing User
        public async Task<User> UpdateUser(User user)
        {
            try
            {
                return await _unitOfWork.Users.Update(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the user: {ex.Message}");
                throw new Exception("Could not update the user.", ex);
            }
        }

        // Delete a User by ID
        public async Task<bool> DeleteUser(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetFirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                await _unitOfWork.Users.Delete(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the user: {ex.Message}");
                throw new Exception("Could not delete the user.", ex);
            }
        }
    }
}

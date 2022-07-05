using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace _20220704_Practice.Models
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options): base(options)
        {

        }
        public DbSet<SignUpModel> SignUpModel { get; set; }
        public DbSet<tblitem> tblitems { get; set; }
    }
}

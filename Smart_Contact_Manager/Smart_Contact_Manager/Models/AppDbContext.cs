using Smart_Contact_Manager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SmartContactManager.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=myConnectionString")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
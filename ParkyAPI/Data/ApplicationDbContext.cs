﻿using Microsoft.EntityFrameworkCore;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<NPark> NParks { get; set; }  //for creation of Npark tables
        public DbSet<Trail> Trails { get; set; }  //for creation of trail tables
    }
}

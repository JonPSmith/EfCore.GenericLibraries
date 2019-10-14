﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Tests.EfClasses;

namespace Tests.EfCode
{
    public class TestDbContext : DbContext
    {
        public DbSet<NormalEntity> NormalEntities { get; set; }
        public DbSet<DddCtorEntity> DddCtorEntities { get; set; }
        public DbSet<DddStaticCreateEntity> DddStaticFactEntities { get; set; }
        public DbSet<DddCtorAndFactEntity> DddCtorAndFactEntities { get; set; }
        public DbSet<NotUpdatableEntity> NotUpdatableEntities { get; set; }
        public DbSet<ReadOnlyEntity> ReadOnlyEntities { get; set; }
        public DbSet<UniqueEntity> UniqueEntities { get; set; }
        public DbSet<TenantAddress> TenantAddresses { get; set; }
        public DbSet<ContactAddress> ContactAddresses { get; set; }
        public DbSet<KeyIsString> KeyIsStrings { get; set; }
        public DbSet<DddCompositeIntString> DddCompositeIntStrings { get; set; }
        public DbSet<SoftDelEntity> SoftDelEntities { get; set; }

        public DbSet<TestAbstractMain> TestAbstractMains { get; set; }

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> WritableChildren { get; set; }
#if NETCOREAPP2_1
        public DbQuery<ChildReadOnly> Children { get; set; }
#elif NETCOREAPP3_0
       public DbSet<ChildReadOnly> Children { get; set; }
#endif


        public TestDbContext(DbContextOptions<TestDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DddCompositeIntString>().HasKey(p => new {p.MyInt, p.MyString});
            modelBuilder.Entity<UniqueEntity>().HasIndex(p => p.UniqueString).IsUnique().HasName("UniqueError_UniqueEntity_UniqueString");

            modelBuilder.Entity<SoftDelEntity>().HasQueryFilter(x => !x.SoftDeleted);

#if NETCOREAPP2_1
            modelBuilder.Entity<Child>().ToTable(nameof(Children));
#elif NETCOREAPP3_0
            modelBuilder.Entity<ChildReadOnly>().ToView("ChildrenView").HasNoKey();
#endif
        }
    }
}
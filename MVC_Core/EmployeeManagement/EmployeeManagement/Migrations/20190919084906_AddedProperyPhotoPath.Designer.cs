﻿// <auto-generated />
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmployeeManagement.Migrations
{
    [DbContext(typeof(AppDbContrext))]
    [Migration("20190919084906_AddedProperyPhotoPath")]
    partial class AddedProperyPhotoPath
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EmployeeManagement.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Department");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PhotoPath");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Department = 2,
                            Email = "marry1@gmail.com",
                            Name = "Marry1"
                        },
                        new
                        {
                            Id = 2,
                            Department = 3,
                            Email = "marry2@gmail.com",
                            Name = "Marry2"
                        },
                        new
                        {
                            Id = 3,
                            Department = 4,
                            Email = "marry3@gmail.com",
                            Name = "Marry3"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

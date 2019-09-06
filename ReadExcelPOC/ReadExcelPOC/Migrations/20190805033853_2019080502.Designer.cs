﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReadExcelPOC.Models;

namespace ReadExcelPOC.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20190805033853_2019080502")]
    partial class _2019080502
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReadExcelPOC.Models.City", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<string>("CityName");

                    b.Property<string>("CountryName");

                    b.Property<string>("GEOID");

                    b.Property<string>("RKSTCode");

                    b.Property<string>("StandardName");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("ReadExcelPOC.Models.Terminal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ExtendField1");

                    b.Property<string>("ExtendField2");

                    b.Property<string>("ExtendField3");

                    b.Property<string>("ExtendField4");

                    b.Property<string>("ExtendField5");

                    b.Property<string>("ExtendField6");

                    b.Property<string>("ExtendField7");

                    b.Property<string>("ExtendField8");

                    b.Property<string>("PortGEOID");

                    b.Property<string>("PortName");

                    b.Property<string>("PortRKST");

                    b.Property<string>("SubArea")
                        .HasColumnName("Sub Area");

                    b.Property<string>("TerminalGEOID");

                    b.Property<string>("TerminalName");

                    b.Property<string>("TerminalRKSTCode");

                    b.HasKey("Id");

                    b.ToTable("Terminal");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using TMCS.Server.Models;

namespace TMCS.Server.Migrations
{
    [DbContext(typeof(TMCSServerContext))]
    [Migration("20170914100213_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("TMCS.Server.Models.Contact", b =>
                {
                    b.Property<string>("SubjectId");

                    b.Property<string>("ObjectId");

                    b.Property<string>("Note");

                    b.HasKey("SubjectId", "ObjectId");

                    b.HasIndex("ObjectId");

                    b.HasIndex("SubjectId")
                        .IsUnique();

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("TMCS.Server.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("AddContactPermission");

                    b.Property<bool>("BlockStranger");

                    b.Property<string>("NickName");

                    b.Property<string>("PasswordHash");

                    b.Property<byte[]>("PrivateKeyEncrypted");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired();

                    b.Property<string>("Salt");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TMCS.Server.Models.Contact", b =>
                {
                    b.HasOne("TMCS.Server.Models.User", "Object")
                        .WithMany("Contacts")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TMCS.Server.Models.User", "Subject")
                        .WithOne()
                        .HasForeignKey("TMCS.Server.Models.Contact", "SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
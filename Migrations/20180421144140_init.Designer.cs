﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using MovinderAPI.Models;
using System;

namespace MovinderAPI.Migrations
{
    [DbContext(typeof(MovinderContext))]
    [Migration("20180421144140_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("MovinderAPI.Models.Invitaiton", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("cinema")
                        .IsRequired();

                    b.Property<string>("city")
                        .IsRequired();

                    b.Property<long>("inviterId");

                    b.Property<string>("movie")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("inviterId");

                    b.ToTable("Invitaiton");
                });

            modelBuilder.Entity("MovinderAPI.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("city")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("email")
                        .IsRequired();

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<byte[]>("passwordHash");

                    b.Property<byte[]>("passwordSalt");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MovinderAPI.Models.Invitaiton", b =>
                {
                    b.HasOne("MovinderAPI.Models.User", "inviter")
                        .WithMany("InvaterPosts")
                        .HasForeignKey("inviterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

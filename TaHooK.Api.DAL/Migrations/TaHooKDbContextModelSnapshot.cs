﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaHooK.Api.DAL;

#nullable disable

namespace TaHooK.Api.DAL.Migrations
{
    [DbContext(typeof(TaHooKDbContext))]
    partial class TaHooKDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.AnswerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.QuestionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuizTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuizTemplateId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.QuizEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Finished")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("TemplateId");

                    b.ToTable("Quizes");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.QuizTemplateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("QuizTemplates");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.ScoreEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuizId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.HasIndex("UserId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.AnswerEntity", b =>
                {
                    b.HasOne("TaHooK.Api.DAL.Entities.QuestionEntity", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.QuestionEntity", b =>
                {
                    b.HasOne("TaHooK.Api.DAL.Entities.QuizTemplateEntity", "QuizTemplate")
                        .WithMany("Questions")
                        .HasForeignKey("QuizTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuizTemplate");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.QuizEntity", b =>
                {
                    b.HasOne("TaHooK.Api.DAL.Entities.UserEntity", "Creator")
                        .WithMany("Quizes")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("TaHooK.Api.DAL.Entities.QuizTemplateEntity", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.QuizTemplateEntity", b =>
                {
                    b.HasOne("TaHooK.Api.DAL.Entities.UserEntity", "Creator")
                        .WithMany("QuizTemplates")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.ScoreEntity", b =>
                {
                    b.HasOne("TaHooK.Api.DAL.Entities.QuizEntity", "Quiz")
                        .WithMany("Scores")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaHooK.Api.DAL.Entities.UserEntity", "User")
                        .WithMany("Scores")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.QuestionEntity", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.QuizEntity", b =>
                {
                    b.Navigation("Scores");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.QuizTemplateEntity", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("TaHooK.Api.DAL.Entities.UserEntity", b =>
                {
                    b.Navigation("QuizTemplates");

                    b.Navigation("Quizes");

                    b.Navigation("Scores");
                });
#pragma warning restore 612, 618
        }
    }
}

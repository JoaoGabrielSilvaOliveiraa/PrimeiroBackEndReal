﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using beyondthegame.Models;

#nullable disable

namespace numguentomais.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20231028002645_teste")]
    partial class teste
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("beyondthegame.Controllers.biblioteca", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("jogo_id")
                        .HasColumnType("int");

                    b.Property<int>("usuario_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("jogo_id");

                    b.HasIndex("usuario_id");

                    b.ToTable("biblioteca");
                });

            modelBuilder.Entity("beyondthegame.Models.empresa", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("cnpj")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cpf")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("numero_de_contato")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("senha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("empresa");
                });

            modelBuilder.Entity("beyondthegame.Models.seguidores", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("empresa_id")
                        .HasColumnType("int");

                    b.Property<int>("estado")
                        .HasColumnType("int");

                    b.Property<int>("usuario_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("empresa_id");

                    b.HasIndex("usuario_id");

                    b.ToTable("seguidores");
                });

            modelBuilder.Entity("beyondthegame.Models.usuario", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("senha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("usuario");
                });

            modelBuilder.Entity("jogo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("classificacao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("empresa_id")
                        .HasColumnType("int");

                    b.Property<string>("link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("empresa_id");

                    b.ToTable("jogo");
                });

            modelBuilder.Entity("beyondthegame.Controllers.biblioteca", b =>
                {
                    b.HasOne("jogo", "jogo")
                        .WithMany()
                        .HasForeignKey("jogo_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("beyondthegame.Models.usuario", "usuario")
                        .WithMany()
                        .HasForeignKey("usuario_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("jogo");

                    b.Navigation("usuario");
                });

            modelBuilder.Entity("beyondthegame.Models.seguidores", b =>
                {
                    b.HasOne("beyondthegame.Models.empresa", "empresa")
                        .WithMany()
                        .HasForeignKey("empresa_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("beyondthegame.Models.usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("usuario_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");

                    b.Navigation("empresa");
                });

            modelBuilder.Entity("jogo", b =>
                {
                    b.HasOne("beyondthegame.Models.empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("empresa_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Empresa");
                });
#pragma warning restore 612, 618
        }
    }
}

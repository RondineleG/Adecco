﻿using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Adecco.Persistence.Migrations;

[DbContext(typeof(ApplicattionDataContext))]
internal partial class ApplicattionDataContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

        modelBuilder.Entity(
            "Adecco.Core.Entities.Cliente",
            b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");

                b.Property<string>("CPF").IsRequired().HasMaxLength(11).HasColumnType("TEXT").IsFixedLength();

                b.Property<int>("ContatoId").HasColumnType("INTEGER");

                b.Property<string>("Email").IsRequired().HasMaxLength(50).HasColumnType("TEXT");

                b.Property<int>("EnderecoId").HasColumnType("INTEGER");

                b.Property<string>("Nome").IsRequired().HasMaxLength(50).HasColumnType("TEXT");

                b.Property<string>("RG").IsRequired().HasMaxLength(11).HasColumnType("TEXT").IsFixedLength();

                b.HasKey("Id");

                b.ToTable("Clientes", (string)null);
            });

        modelBuilder.Entity(
            "Adecco.Core.Entities.Contato",
            b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");

                b.Property<int?>("ClienteId").HasColumnType("INTEGER");

                b.Property<int>("DDD").HasMaxLength(2).HasColumnType("INTEGER").IsFixedLength();

                b.Property<string>("Nome").IsRequired().HasMaxLength(50).HasColumnType("TEXT");

                b.Property<decimal>("Telefone").HasMaxLength(9).HasColumnType("TEXT");

                b.Property<byte>("TipoContato").HasColumnType("INTEGER");

                b.HasKey("Id");

                b.HasIndex("ClienteId");

                b.ToTable("Contatos", (string)null);
            });

        modelBuilder.Entity(
            "Adecco.Core.Entities.Endereco",
            b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");

                b.Property<string>("Bairro").IsRequired().HasMaxLength(30).HasColumnType("TEXT");

                b.Property<string>("CEP").IsRequired().HasMaxLength(8).HasColumnType("TEXT").IsFixedLength();

                b.Property<string>("Cidade").IsRequired().HasMaxLength(50).HasColumnType("TEXT");

                b.Property<int?>("ClienteId").HasColumnType("INTEGER");

                b.Property<string>("Complemento").HasMaxLength(30).HasColumnType("TEXT");

                b.Property<string>("Estado").IsRequired().HasMaxLength(2).HasColumnType("TEXT");

                b.Property<string>("Logradouro").IsRequired().HasMaxLength(50).HasColumnType("TEXT");

                b.Property<string>("Nome").IsRequired().HasMaxLength(50).HasColumnType("TEXT");

                b.Property<string>("Numero").IsRequired().HasMaxLength(20).HasColumnType("TEXT");

                b.Property<string>("Referencia").HasMaxLength(50).HasColumnType("TEXT");

                b.Property<byte>("TipoEndereco").HasColumnType("INTEGER");

                b.HasKey("Id");

                b.HasIndex("ClienteId");

                b.ToTable("Enderecos", (string)null);
            });

        modelBuilder.Entity(
            "Adecco.Core.Entities.Contato",
            b =>
            {
                b.HasOne("Adecco.Core.Entities.Cliente", "Cliente")
                    .WithMany("Contatos")
                    .HasForeignKey("ClienteId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.Navigation("Cliente");
            });

        modelBuilder.Entity(
            "Adecco.Core.Entities.Endereco",
            b =>
            {
                b.HasOne("Adecco.Core.Entities.Cliente", "Cliente")
                    .WithMany("Enderecos")
                    .HasForeignKey("ClienteId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.Navigation("Cliente");
            });

        modelBuilder.Entity(
            "Adecco.Core.Entities.Cliente",
            b =>
            {
                b.Navigation("Contatos");

                b.Navigation("Enderecos");
            });
#pragma warning restore 612, 618
    }
}
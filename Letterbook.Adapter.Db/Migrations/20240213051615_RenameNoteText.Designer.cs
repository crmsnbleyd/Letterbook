﻿// <auto-generated />
using System;
using Letterbook.Adapter.Db;
using Letterbook.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Letterbook.Adapter.Db.Migrations
{
    [DbContext(typeof(RelationalContext))]
    [Migration("20240213051615_RenameNoteText")]
    partial class RenameNoteText
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AudienceProfileMembers", b =>
                {
                    b.Property<Guid>("AudiencesId")
                        .HasColumnType("uuid");

                    b.Property<string>("MembersFediId")
                        .HasColumnType("text");

                    b.HasKey("AudiencesId", "MembersFediId");

                    b.HasIndex("MembersFediId");

                    b.ToTable("AudienceProfileMembers");
                });

            modelBuilder.Entity("ImagesCreatedByProfile", b =>
                {
                    b.Property<Guid>("CreatedImagesId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatorsFediId")
                        .HasColumnType("text");

                    b.HasKey("CreatedImagesId", "CreatorsFediId");

                    b.HasIndex("CreatorsFediId");

                    b.ToTable("ImagesCreatedByProfile");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    b.HasIndex("UserName");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Audience", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("FediId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uuid");

                    b.Property<string>("SourceFediId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FediId");

                    b.HasIndex("ImageId");

                    b.HasIndex("SourceFediId");

                    b.ToTable("Audience");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Content", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<string>("FediId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<string>("Preview")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FediId");

                    b.HasIndex("PostId");

                    b.ToTable("Content");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Content");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Letterbook.Core.Models.FollowerRelation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FollowerFediId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FollowsFediId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Date");

                    b.HasIndex("FollowerFediId");

                    b.HasIndex("FollowsFediId");

                    b.ToTable("FollowerRelation");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FediId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileLocation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Letterbook.Core.Models.LinkedProfile", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Permission")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("ProfileFediId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AccountId", "ProfileId");

                    b.HasIndex("ProfileFediId");

                    b.ToTable("LinkedProfile");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Authority")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Client")
                        .HasColumnType("text");

                    b.Property<string>("ContentRootIdUri")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FediId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("InReplyToId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("LastSeenDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Likes")
                        .HasColumnType("text");

                    b.Property<string>("Preview")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("PublishedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Replies")
                        .HasColumnType("text");

                    b.Property<string>("Shares")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<Guid?>("ThreadId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ContentRootIdUri");

                    b.HasIndex("FediId");

                    b.HasIndex("InReplyToId");

                    b.HasIndex("ThreadId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Profile", b =>
                {
                    b.Property<string>("FediId")
                        .HasColumnType("text");

                    b.Property<CustomField[]>("CustomFields")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Followers")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Following")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Inbox")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Outbox")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("OwnedById")
                        .HasColumnType("uuid");

                    b.Property<string>("SharedInbox")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("FediId");

                    b.HasIndex("Id");

                    b.HasIndex("OwnedById");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("Letterbook.Core.Models.SigningKey", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Family")
                        .HasColumnType("integer");

                    b.Property<string>("FediId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("KeyOrder")
                        .HasColumnType("integer");

                    b.Property<string>("Label")
                        .HasColumnType("text");

                    b.Property<byte[]>("PrivateKey")
                        .HasColumnType("bytea");

                    b.Property<string>("ProfileFediId")
                        .HasColumnType("text");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.HasIndex("ProfileFediId");

                    b.ToTable("SigningKey");
                });

            modelBuilder.Entity("Letterbook.Core.Models.ThreadContext", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("FediId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RootId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FediId");

                    b.HasIndex("RootId");

                    b.ToTable("ThreadContext");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", "AspnetIdentity");
                });

            modelBuilder.Entity("PostsCreatedByProfile", b =>
                {
                    b.Property<Guid>("CreatedPostsId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatorsFediId")
                        .HasColumnType("text");

                    b.HasKey("CreatedPostsId", "CreatorsFediId");

                    b.HasIndex("CreatorsFediId");

                    b.ToTable("PostsCreatedByProfile");
                });

            modelBuilder.Entity("PostsLikedByProfile", b =>
                {
                    b.Property<Guid>("LikedPostsId")
                        .HasColumnType("uuid");

                    b.Property<string>("LikesCollectionFediId")
                        .HasColumnType("text");

                    b.HasKey("LikedPostsId", "LikesCollectionFediId");

                    b.HasIndex("LikesCollectionFediId");

                    b.ToTable("PostsLikedByProfile");
                });

            modelBuilder.Entity("PostsSharedByProfile", b =>
                {
                    b.Property<Guid>("SharedPostsId")
                        .HasColumnType("uuid");

                    b.Property<string>("SharesCollectionFediId")
                        .HasColumnType("text");

                    b.HasKey("SharedPostsId", "SharesCollectionFediId");

                    b.HasIndex("SharesCollectionFediId");

                    b.ToTable("PostsSharedByProfile");
                });

            modelBuilder.Entity("PostsToAudience", b =>
                {
                    b.Property<Guid>("AudienceId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.HasKey("AudienceId", "PostId");

                    b.HasIndex("PostId");

                    b.ToTable("PostsToAudience");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Note", b =>
                {
                    b.HasBaseType("Letterbook.Core.Models.Content");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Note");
                });

            modelBuilder.Entity("AudienceProfileMembers", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Audience", null)
                        .WithMany()
                        .HasForeignKey("AudiencesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("MembersFediId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImagesCreatedByProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Image", null)
                        .WithMany()
                        .HasForeignKey("CreatedImagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("CreatorsFediId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Letterbook.Core.Models.Audience", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Image", null)
                        .WithMany("Visibility")
                        .HasForeignKey("ImageId");

                    b.HasOne("Letterbook.Core.Models.Profile", "Source")
                        .WithMany()
                        .HasForeignKey("SourceFediId");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Content", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Post", "Post")
                        .WithMany("Contents")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Letterbook.Core.Models.FollowerRelation", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Profile", "Follower")
                        .WithMany("FollowingCollection")
                        .HasForeignKey("FollowerFediId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", "Follows")
                        .WithMany("FollowersCollection")
                        .HasForeignKey("FollowsFediId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Follower");

                    b.Navigation("Follows");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Image", b =>
                {
                    b.OwnsMany("Letterbook.Core.Models.Mention", "Mentions", b1 =>
                        {
                            b1.Property<Guid>("ImageId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<string>("SubjectFediId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Visibility")
                                .HasColumnType("integer");

                            b1.HasKey("ImageId", "Id");

                            b1.HasIndex("SubjectFediId");

                            b1.ToTable("Images_Mentions");

                            b1.WithOwner()
                                .HasForeignKey("ImageId");

                            b1.HasOne("Letterbook.Core.Models.Profile", "Subject")
                                .WithMany()
                                .HasForeignKey("SubjectFediId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.Navigation("Subject");
                        });

                    b.Navigation("Mentions");
                });

            modelBuilder.Entity("Letterbook.Core.Models.LinkedProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Account", "Account")
                        .WithMany("LinkedProfiles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", "Profile")
                        .WithMany("RelatedAccounts")
                        .HasForeignKey("ProfileFediId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Post", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Post", "InReplyTo")
                        .WithMany("RepliesCollection")
                        .HasForeignKey("InReplyToId");

                    b.HasOne("Letterbook.Core.Models.ThreadContext", "Thread")
                        .WithMany("Posts")
                        .HasForeignKey("ThreadId");

                    b.OwnsMany("Letterbook.Core.Models.Mention", "AddressedTo", b1 =>
                        {
                            b1.Property<Guid>("PostId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<string>("SubjectFediId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Visibility")
                                .HasColumnType("integer");

                            b1.HasKey("PostId", "Id");

                            b1.HasIndex("SubjectFediId");

                            b1.ToTable("Post_AddressedTo");

                            b1.WithOwner()
                                .HasForeignKey("PostId");

                            b1.HasOne("Letterbook.Core.Models.Profile", "Subject")
                                .WithMany()
                                .HasForeignKey("SubjectFediId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.Navigation("Subject");
                        });

                    b.Navigation("AddressedTo");

                    b.Navigation("InReplyTo");

                    b.Navigation("Thread");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Profile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Account", "OwnedBy")
                        .WithMany()
                        .HasForeignKey("OwnedById");

                    b.Navigation("OwnedBy");
                });

            modelBuilder.Entity("Letterbook.Core.Models.SigningKey", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany("Keys")
                        .HasForeignKey("ProfileFediId");
                });

            modelBuilder.Entity("Letterbook.Core.Models.ThreadContext", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Post", "Root")
                        .WithMany()
                        .HasForeignKey("RootId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Root");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostsCreatedByProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("CreatedPostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("CreatorsFediId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostsLikedByProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("LikedPostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("LikesCollectionFediId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostsSharedByProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("SharedPostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("SharesCollectionFediId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostsToAudience", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Audience", null)
                        .WithMany()
                        .HasForeignKey("AudienceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Letterbook.Core.Models.Account", b =>
                {
                    b.Navigation("LinkedProfiles");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Image", b =>
                {
                    b.Navigation("Visibility");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Post", b =>
                {
                    b.Navigation("Contents");

                    b.Navigation("RepliesCollection");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Profile", b =>
                {
                    b.Navigation("FollowersCollection");

                    b.Navigation("FollowingCollection");

                    b.Navigation("Keys");

                    b.Navigation("RelatedAccounts");
                });

            modelBuilder.Entity("Letterbook.Core.Models.ThreadContext", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}

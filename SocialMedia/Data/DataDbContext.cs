using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Xml;

namespace SocialMedia.Data
{
    public class DataDbContext : IdentityDbContext<User>
    {
        public DataDbContext(DbContextOptions dbContextOptions):base (dbContextOptions)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment>Comments { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<PostLikes> PostLikes { get; set; }

        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var readerGuid = "376be536-4dd7-4a46-9b3a-062548c8acd8";
            var writerGuid = "552b36f8-3fb4-461d-8a62-a2f430e29330";
            base.OnModelCreating(builder);
            var roles = new List<IdentityRole>{
                new IdentityRole
                {
                    Id=readerGuid,
                    ConcurrencyStamp=readerGuid,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id=writerGuid,
                    ConcurrencyStamp=writerGuid,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper()
                }
            };




            builder.Entity<IdentityRole>().HasData(roles);


           

            builder.Entity<Comment>().HasOne(p => p.Post).WithMany(u => u.Comments).HasForeignKey(p => p.PostId);

            builder.Entity<Friends>()
            .HasIndex(e => new { e.FirstId, e.SecondId })
            .IsUnique(true);
            


        }




    }
}

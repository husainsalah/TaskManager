using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  
using Microsoft.EntityFrameworkCore;  

namespace TaskManager.Models{  
    public class AppDbContext : IdentityDbContext
        {
            private readonly DbContextOptions _options;

            public AppDbContext(DbContextOptions options): base(options)
            {
                _options = options; 
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                using (var context = new AppDbContext(_options))
                {
                    context.TaskListItems.ToListAsync();
                    //   .OrderByDescending(x => x.DueDate).ToList();
                    //var list = context.TaskListItems.ToListAsync();
                }
                
            }
            //entities
            public DbSet<TaskListItem> TaskListItems { get; set; }
        
        }
}
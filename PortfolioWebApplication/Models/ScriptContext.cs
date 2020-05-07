using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PortfolioWebApplication.Models
{
    public class ScriptContext : DbContext
    {
        public ScriptContext(DbContextOptions<ScriptContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScriptContext() { }

        #region Delegates & Events

        public delegate bool ScriptAddedCB(Script script);
        public event ScriptAddedCB ScriptAdded;
        public delegate bool ScriptDeletedCB(Script script);
        public event ScriptDeletedCB ScriptDeleted;

        #endregion

        public DbSet<Script> ScriptItems { get; set; }

        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            return base.AddAsync(entity, cancellationToken);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            EntityEntry < TEntity > entityEntry = base.Add(entity);
            try
            {
                SaveChanges();
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            return entityEntry;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        //    //base.OnConfiguring(optionsBuilder);
        //    //optionsBuilder.UseSqlServer(@"Server=BRIANCLAPTOP;Database=ArtificialIntelligence;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(@"Server=BRIANCLAPTOP;Database=ArtificialIntelligence;User ID=sa;Password=lakeview828;");
        }
    }
}

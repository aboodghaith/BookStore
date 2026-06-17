using DAL.Data;
using DAL.Models;
using DAL.Repository.Implementations;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository UserRepository { get; private  set; }
        public IRepository<Book> BookRepository { get; private set; }

        public IRepository<Category> CategoryRepository { get; private set; }

        public IRepository<Cart> CartRepository { get; private set; }

        public IRepository<CartItem> CartItemRepository { get; private set; }

        public IRepository<OrderItem> OrderItemRepository { get; private set ; }

        public IRepository<Order> OrderRepository { get; private set; }

        public IRepository<Payment> PaymentRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            UserRepository = new UserRepository(context);

            BookRepository = new BaseRepository<Book>(context);

            CategoryRepository = new BaseRepository<Category>(context); 

            CartItemRepository = new BaseRepository<CartItem>(context);

            CartRepository = new BaseRepository<Cart>(context);

            OrderRepository = new BaseRepository<Order>(context); 

            OrderItemRepository = new BaseRepository<OrderItem>(context); 

            PaymentRepository = new BaseRepository<Payment>(context);
        }

        

        public Task<int> SaveChanges()
        {
            return _context.SaveChangesAsync();

        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}

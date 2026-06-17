using DAL.Models;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {  get; }
        IRepository<Book> BookRepository { get; }

        IRepository<Category> CategoryRepository { get; }

        IRepository<Cart> CartRepository { get; }

        IRepository<CartItem> CartItemRepository { get; }

        IRepository<OrderItem> OrderItemRepository { get; }

        IRepository<Order> OrderRepository { get; }

        IRepository<Payment> PaymentRepository { get; }



        public Task<int> SaveChanges();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}

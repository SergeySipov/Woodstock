﻿using System;
using System.Linq;
using Woodstock.BLL.DTOs;
using Woodstock.BLL.Extensions;
using Woodstock.BLL.Interfaces;
using Woodstock.DAL;
using Woodstock.DAL.Entities;

namespace Woodstock.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly WoodstockDbContext _context;

        public OrderService(WoodstockDbContext context)
        {
            _context = context;
        }

        private Order CreateOrder(int userId)
        {
            var order = new Order
            {
                UserId = userId,
                PaymentMethod = "Наличные",
                OrderDate = DateTime.Now,
                User = _context.Users.Find(userId),
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }

        public void AddItemsToOrder(int userId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var carts = _context.ShoppingCarts.Where(_ => _.UserId == userId && _.IsChecked).ToList();
                    if (carts.Count > 0)
                    {
                        var order = CreateOrder(userId);

                        var oderLinks = carts.Select(_ => _.ToOrder(order));
                        _context.OrderWatchLinks.AddRange(oderLinks);
                        _context.ShoppingCarts.RemoveRange(carts);

                        _context.Watches.UpdateRange(carts.Select(_ =>
                        {
                            _.Watch.CountInStock = _.Watch.CountInStock - _.Count;
                            return _.Watch;
                        }));

                        order.TotalPrice = carts.Sum(_ => _.Count * _.Watch.Price);
                        order.TotalCount = carts.Sum(_ => _.Count);

                        _context.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public IQueryable<OrderDTO> GetOrders(int userId)
        {
            return from order in _context.Orders
                   where order.UserId == userId && order.IsOrderCompleted != true
                   orderby order.Id descending
                   select order.ToDTO();
        }
    }
}

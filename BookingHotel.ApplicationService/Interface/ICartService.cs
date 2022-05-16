using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookingHotel.ApplicationService.BaseServices;
using BookingHotel.Common;
using BookingHotel.Model.Auth;
using BookingHotel.Model.Catalog;

namespace BookingHotel.ApplicationService.Interface
{
    public interface ICartService : IBaseService<CartModel>
    {
        Task<CartModel> FindById(int accountId, int productId);
        Task<ProcessingResult> DeleteAsync(int accountId, int productId);
        Task<ProcessingResult> DeleteByAccountAsync(int accountId);
        Task<List<CartModel>> GetByAccountAsync(int accountId);
        Task<ProcessingResult> UpdateQuantityAsync(int accountId, int productId, int qty);

        Task<CartModel> AddItemintoCart(CartModel basketItem);
        Task<List<CartModel>> ChangeCartItemQuantity(int accountId, int productId);
        Task<List<CartModel>> UpdateCartItemQuantity(int accountId, int productId, int quantity);
        Task<List<CartModel>> ClearCart(int accountId);
        Task<List<CartModel>> DeleteCartItem(int accountId, int productId);
        Task<List<CartModel>> GetCartItems(int accountId);
    }
}

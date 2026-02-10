using Entertainment_travel_booking_website.Models;
using System.Collections.Generic;

public interface ICartRepository
{
    void AddToCart(string userId, int tripId, List<int> activityIds, decimal totalPrice, int quantity);
    List<CartItem> GetCartItems(string userId);

    void RemoveCartItem(int cartItemId);

    void ClearCart(string userId);
}

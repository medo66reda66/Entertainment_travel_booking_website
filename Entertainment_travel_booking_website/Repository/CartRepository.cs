using Entertainment_travel_booking_website.DataBase;
using Entertainment_travel_booking_website.Models;
using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddToCart(string userId, int tripId, List<int> activityIds, decimal totalPrice)
    {
        var cartItem = new CartItem
        {
            UserId = userId,
            TripId = tripId,
            TotalPrice = totalPrice,
            // هنا بنربط الأنشطة المختارة (تأكد أن الموديل يدعم هذه العلاقة)
            SelectedActivities = _context.additianActivites
                                    .Where(a => activityIds.Contains(a.Id)).ToList()
        };

        _context.cartItems.Add(cartItem);
        _context.SaveChanges(); // <-- ده أهم سطر، بدونه مفيش حاجة بتتحفظ!
    }

    public List<CartItem> GetCartItems(string userId)
    {
        return _context.cartItems
            .Include(c => c.Trip) // مهم جداً لعرض اسم الرحلة
            .Include(c => c.SelectedActivities) // مهم لعرض الأنشطة
            .Where(c => c.UserId == userId)
            .ToList();
    }

    public void RemoveCartItem(int cartItemId)
    {
        var item = _context.cartItems.Find(cartItemId);
        if (item != null)
        {
            _context.cartItems.Remove(item);
            _context.SaveChanges();
        }
    }

    public void ClearCart(string userId)
    {
        var items = _context.cartItems.Where(c => c.UserId == userId);
        _context.cartItems.RemoveRange(items);
        _context.SaveChanges();
    }
}
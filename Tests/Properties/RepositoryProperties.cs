using FsCheck;
using FsCheck.Xunit;
using Shopping_Cart_System.Domain;
using Shopping_Cart_System.Tests.Arbitraries;

namespace Shopping_Cart_System.Tests.Properties
{
    public class RepositoryProperties
    {
        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void SaveCart_AddsCartToStorage(ShoppingCart cart)
        {
            // Arrange
            var repo = new InMemoryCartRepository();
            var service = new CartService(repo);

            // Act
            service.SaveCart(cart);

            // Assert
            Assert.Contains(cart, repo.All());
            Assert.Single(repo.All());
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void SaveCart_MultipleSaves_KeepAllCarts(ShoppingCart[] carts)
        {
            // Arrange
            var repo = new InMemoryCartRepository();
            var service = new CartService(repo);

            // Act
            foreach (var cart in carts)
            {
                service.SaveCart(cart);
            }

            // Assert
            Assert.Equal(carts.Length, repo.All().Count);
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void Save_SavedCartDataRemainsConsistent(ShoppingCart cart)
        {
            // Arrange
            var repo = new InMemoryCartRepository();
            
            // Act
            repo.Save(cart);

            var savedCart = repo.All().First();
            
            // Assert
            Assert.Equal(cart.TotalPrice(), savedCart.TotalPrice());
            Assert.Equal(cart.Items.Count, savedCart.Items.Count);
        }
    }
}

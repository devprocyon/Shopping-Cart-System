using FsCheck;
using FsCheck.Xunit;
using Shopping_Cart_System.Domain;
using Shopping_Cart_System.Tests.Arbitraries;

namespace Shopping_Cart_System.Tests.Properties
{
    public class CartProperties
    {
        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void TotalPrice_MatchesSumOfItems(ShoppingCart cart)
        {
            // Arrange
            var expectedTotal = cart.Items.Sum(i => i.Product.Price * i.Quantity);
            
            // Act & Assert
            Assert.Equal(expectedTotal, cart.TotalPrice());
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void Add_NewProduct_IncrementsItemsCount(ShoppingCart cart, Product product, int quantity)
        {
            // Arrange
            if (cart.Items.Any(i => i.Product.Id == product.Id)) return;

            var initialCount = cart.Items.Count;

            // Act
            cart.Add(product, quantity);

            // Assert
            Assert.Equal(initialCount + 1, cart.Items.Count);
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void Add_ExistingProduct_DoesNotChangeItemsCountButUpdatesQuantity(ShoppingCart cart, int quantity)
        {
            // Arrange
            if (!cart.Items.Any()) return;
            
            var existingItem = cart.Items.First();
            var initialQty = existingItem.Quantity;
            var initialCount = cart.Items.Count;

            // Act
            cart.Add(existingItem.Product, quantity);

            // Assert
            Assert.Equal(initialCount, cart.Items.Count);
            Assert.Equal(initialQty + quantity, existingItem.Quantity);
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void Remove_ExistingProduct_DecrementsItemsCount(ShoppingCart cart)
        {
            // Arrange
            if (!cart.Items.Any()) return;

            var productToRemove = cart.Items.First().Product;
            var initialCount = cart.Items.Count;

            // Act
            cart.Remove(productToRemove.Id);

            // Assert
            Assert.Equal(initialCount - 1, cart.Items.Count);
            Assert.DoesNotContain(cart.Items, i => i.Product.Id == productToRemove.Id);
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void TotalPrice_IsNeverNegative(ShoppingCart cart)
        {
            // Arrange & Act & Assert
            Assert.True(cart.TotalPrice() >= 0);
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void Add_OperationIsCommutative_ForTotalPrice(Product p1, int q1, Product p2, int q2)
        {
            // Arrange
            var cart1 = new ShoppingCart();
            var cart2 = new ShoppingCart();
            
            // Act
            cart1.Add(p1, q1);
            cart1.Add(p2, q2);

            cart2.Add(p2, q2);
            cart2.Add(p1, q1);

            // Assert
            Assert.Equal(cart1.TotalPrice(), cart2.TotalPrice());
        }
    }
}

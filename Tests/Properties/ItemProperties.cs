using FsCheck;
using FsCheck.Xunit;
using Shopping_Cart_System.Domain;
using Shopping_Cart_System.Tests.Arbitraries;

namespace Shopping_Cart_System.Tests.Properties
{
    public class ItemProperties
    {
        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void Constructor_AlwaysSetsPropertiesCorrectly(Product product, int quantity)
        {
            // Act
            var item = new CartItem(product, quantity);

            // Assert
            Assert.Equal(product, item.Product);
            Assert.Equal(quantity, item.Quantity);
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void Increase_AlwaysAddsExactAmount(Product product, int initialQty, int addAmount)
        {
            // Arrange
            var item = new CartItem(product, initialQty);

            // Act
            item.Increase(addAmount);

            // Assert
            Assert.Equal(initialQty + addAmount, item.Quantity);
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void Decrease_ReducesAmountCorrectly_IfResultIsPositive(Product product, int initialQty, int decreaseAmount)
        {
            // Arrange
            if (decreaseAmount >= initialQty) return; 

            var item = new CartItem(product, initialQty);
            
            // Act
            item.Decrease(decreaseAmount);

            // Assert
            Assert.Equal(initialQty - decreaseAmount, item.Quantity);
        }

        [Property(Arbitrary = new[] { typeof(ShopArbitraries) })]
        public void Decrease_ThrowsException_WhenResultIsZeroOrLess(Product product, int initialQty, int decreaseAmount)
        {
            // Arrange
            if (decreaseAmount < initialQty) return;

            var item = new CartItem(product, initialQty);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => item.Decrease(decreaseAmount));
        }
    }
}

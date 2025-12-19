using FsCheck;
using FsCheck.Fluent;
using Shopping_Cart_System.Domain;

namespace Shopping_Cart_System.Tests.Arbitraries
{
    public static class ShopArbitraries
    {
        public static Arbitrary<Product> Product()
        {
            var genProduct =
                from id in NonEmptyAlphaString()
                from name in NonEmptyAlphaString()
                from price in Gen.Choose(1, 1000000).Select(x => x / 100m) 
                select new Product(id, name, price);

            return genProduct.ToArbitrary();
        }

        public static Arbitrary<int> Quantity()
        {
            return Gen.Choose(1, 100).ToArbitrary();
        }

        public static Arbitrary<ShoppingCart> Cart()
        {
            var genCart =
                from products in Gen.ListOf(Product().Generator)
                let uniqueProducts = products.DistinctBy(p => p.Id).ToList()
                from quantities in Gen.ArrayOf(Quantity().Generator, uniqueProducts.Count)
                select BuildCart(uniqueProducts, quantities.ToList());

            return genCart.ToArbitrary();
        }

        private static ShoppingCart BuildCart(List<Product> products, List<int> quantities)
        {
            var cart = new ShoppingCart();
            for (int i = 0; i < products.Count; i++)
            {
                cart.Add(products[i], quantities[i]);
            }
            return cart;
        }

        private static Gen<string> NonEmptyAlphaString()
        {
            return from length in Gen.Choose(3, 10)
                from chars in Gen.ArrayOf(Gen.Elements("abcdefghijklmnopqrstuvwxyz".ToCharArray()), length)
                select new string(chars);
        }
    }
}

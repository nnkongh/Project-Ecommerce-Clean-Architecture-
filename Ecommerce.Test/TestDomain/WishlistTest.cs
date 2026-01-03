using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Test.TestDomain
{
    
    //public class WishlistTest
    //{
    //    [Fact]
    //    public void AddItem_ShouldAddItem_WhenCreateWishlist()
    //    {
    //        var wishlist = Wishlist.Create("12b", "user1");

    //        wishlist.AddItem(1, "product1");
    //        wishlist.AddItem(2, "product2");

    //        var item = wishlist.Items.Count;
    //        Assert.Equal(2, item);
    //        Assert.Equal("12b", wishlist.UserId);
    //        Assert.Equal("user1", wishlist.Name);
    //    }
    //    [Fact]
    //    public void AddItem_ShouldThrowException_WhenAddItemWithIdAlreadyExists()
    //    {
    //        var wishlist = Wishlist.Create("12b", "user1");
    //        var FirstProductId = 1;
    //        var SecondProductId = 1;


    //        wishlist.AddItem(FirstProductId, "Abc");

    //        Assert.Throws<DuplicateProductException>(() => wishlist.AddItem(SecondProductId, "Abc"));
    //    }
    //    [Fact]
    //    public void RemoveItem_ShouldRemoveItem_WhenItemIsExists()
    //    {
    //        var wishlist = Wishlist.Create("12b", "user1");

    //        wishlist.AddItem(1, "ABC");

    //        wishlist.RemoveItem(1);
    //        var item = wishlist.Items.Count;


    //        Assert.Equal(0, item);
    //        Assert.NotNull(wishlist);
    //        Assert.Empty(wishlist.Items);
    //    }
    //    [Fact]
    //    public void RemoveItem_ShouldRemoveTrueItem_WhenMutipleItemsAreExist()
    //    {
    //        var wishlist = Wishlist.Create("12b", "user1");

    //        wishlist.AddItem(1, "ABC");
    //        wishlist.AddItem(2, "NJC");
    //        wishlist.AddItem(3, "HAS");
    //        wishlist.AddItem(4, "BSA");

    //        wishlist.RemoveItem(1);
    //        var item = wishlist.Items.Count;


    //        Assert.Equal(3, item);
    //        Assert.NotNull(wishlist);
    //        Assert.DoesNotContain(wishlist.Items, x => x.ProductId == 1);
    //        Assert.NotEmpty(wishlist.Items);
    //    }
    //    [Fact]
    //    public void ClearItem_ShouldClearAllItems_WhenMutipleItemsAreExist()
    //    {
    //        var wishlist = Wishlist.Create("12b", "user1");

    //        wishlist.AddItem(1, "ABC");
    //        wishlist.AddItem(2, "NJC");
    //        wishlist.AddItem(3, "HAS");
    //        wishlist.AddItem(4, "BSA");

    //        wishlist.ClearItem();
    //        var item = wishlist.Items.Count;


    //        Assert.Equal(0, item);
    //        Assert.NotNull(wishlist);
    //        Assert.Empty(wishlist.Items);
    //    }
    //}
}

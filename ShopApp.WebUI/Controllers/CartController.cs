using System;
using System.Collections.Generic;
using System.Linq;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Extensions;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IOrderService _orderService;
        private UserManager<User> _userManager;
        public CartController(IOrderService orderService, ICartService cartService, UserManager<User> userManager)
        {
            _cartService = cartService;
            _orderService = orderService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
            
            return View( new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i=>new CartItemModel(){
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = _userManager.GetUserId(User);

            _cartService.AddToCart(userId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.DeleteFromCart(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i=>new CartItemModel(){
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            };
            
            return View(orderModel);
        }
        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            var userId= _userManager.GetUserId(User);
            var cart = _cartService.GetCartByUserId(userId);

            model.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i=>new CartItemModel(){
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            };
            
            var payment = PaymentProcess(model);

            if (ModelState.IsValid)
            {

                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(userId);
                    return View("Success");
                }
                else
                {
                    TempData.Put("message", new AlertMessage(){
                    Title = "Delete Message",
                    Message = $"{payment.ErrorMessage}",
                    AlertType = "danger"
                    });
                }
            }
            
            return View(model);
        }

        private void ClearCart(string userId)
        {
            Console.WriteLine("ClearCart method doest work!");
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();
            order.OrderNumber = new Random().Next(111111,999999).ToString();
            order.PaymentType = EnumPaymentType.CreditCard;
            order.OrderState = EnumOrderState.completed;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = DateTime.Now;
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UserId = userId;
            order.Address = model.Address;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;
            order.Note = model.Note;

            order.OrderItems = new List<Entity.OrderItem>();

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new Entity.OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };

                order.OrderItems = new List<Entity.OrderItem>();
                order.OrderItems.Add(orderItem);
            }

            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "enter your api key here";
            options.SecretKey = "enter your secret key here";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";
                    
            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111,999999999).ToString();
            request.Price = "1.0";
            // request.Price = model.CartModel.TotalPrice().ToString();
            // request.PaidPrice = model.CartModel.TotalPrice().ToString();
            request.PaidPrice = "1,0";
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B" + new Random().Next(111111,999999).ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.CVC;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BYR" + new Random().Next(111111,999999);
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = model.City;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = model.Address;
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Emre Tetik";
            billingAddress.City = model.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = model.Address;
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "ShopAppProduct";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = item.Price.ToString();
                basketItems.Add(basketItem);
            }

            request.BasketItems = basketItems;

            return Payment.Create(request, options);
            
        }
    }
}
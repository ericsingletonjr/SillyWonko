using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using Microsoft.Extensions.Configuration;
using SillyWonko.Models.ViewModels;

namespace SillyWonko.Models
{
    public class Transaction
    {
        static IConfiguration Configuration;

        public Transaction(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Run(UserViewModel uvm)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = Configuration["AuthNet:Name"],
                ItemElementName = ItemChoiceType.transactionKey,
                Item = Configuration["AuthNet:TransactionKey"]
            };

            var CreditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0718"
            };

            var PaymentType = new paymentType { Item = CreditCard };

            customerAddressType Address = GetAddress(uvm);

            var TransactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                amount = uvm.Order.TotalPrice,
                payment = PaymentType,
                billTo = Address,
                lineItems = CreateLineItems(uvm),
            };

            var request = new createTransactionRequest { transactionRequest = TransactionRequest };

            var controller = new createTransactionController(request);
            controller.Execute();

            var response = controller.GetApiResponse();
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                        Console.WriteLine("Created transaction. Transaction ID: " +
                            response.transactionResponse.transId);
                        Console.WriteLine("Response Code: " +
                            response.transactionResponse.responseCode);
                        Console.WriteLine("Message Code: " +
                            response.transactionResponse.messages[0].code);
                        Console.WriteLine("Description: " +
                            response.transactionResponse.messages[0].description);
                        Console.WriteLine("Success, Auth Code : " +
                            response.transactionResponse.authCode);
                    }
                    else
                    {
                        Console.WriteLine("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Failed Transaction.");

                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        Console.WriteLine("Error Code: " + response.messages.message[0].code);
                        Console.WriteLine("Error message: " + response.messages.message[0].text);
                    }
                }
            }
            else
            {
                Console.WriteLine("Null Response.");
            }
            return "invalid";
        }
        /// <summary>
        /// Method that allows us to create a customerAddressType
        /// dynamically for transactions
        /// </summary>
        /// <param name="uvm">UserViewModel</param>
        /// <returns>CustomerAddressType</returns>
        public customerAddressType GetAddress(UserViewModel uvm)
        {
            customerAddressType Address = new customerAddressType()
            {
                firstName = uvm.User.FirstName,
                lastName = uvm.User.LastName,
                address = $"{uvm.Shipping.Address1} {uvm.Shipping.Address2}",
                city = uvm.Shipping.City,
                state = uvm.Shipping.State,
                zip = "98004"
            };
            return Address;
        }
        /// <summary>
        /// Method that takes in a UserViewModel to access the
        /// appropriate product information from the cartitems.
        /// This allows for us to dynamically create a sales transaction
        /// </summary>
        /// <param name="uvm">UserViewModel</param>
        /// <returns>LineItemType array</returns>
        public lineItemType[] CreateLineItems(UserViewModel uvm)
        {
            var lineItems = new lineItemType[uvm.Cart.CartItems.Count];
            int index = 0;
            foreach(CartItem item in uvm.Cart.CartItems)
            {
                lineItems[index] = new lineItemType
                {
                    itemId = $"{item.Product.ID}",
                    name = item.Product.Name,
                    quantity = item.Quantity,
                    unitPrice = item.Product.Price
                };
                index++;
            }
            return lineItems;
        }
    }
}

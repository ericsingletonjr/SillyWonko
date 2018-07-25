using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using Microsoft.Extensions.Configuration;


namespace SillyWonko.Models
{
    public class Transaction
    {
        static IConfiguration Configuration;

        public Transaction(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string Run()
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = "5vjF78Lws4",
                ItemElementName = ItemChoiceType.transactionKey,
                Item = "5L7qFH89qzwAm84m"
            };

            var CreditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0718"
            };

            var PaymentType = new paymentType { Item = CreditCard };

            customerAddressType Address = new customerAddressType()
            {
                firstName = "Bob",
                lastName = "Dole",
                address = "123 not real lane",
                city = "FakeCity",
                zip = "98004"
            };

            var LineItems = new lineItemType[2]
            {
               new lineItemType
               {
                   itemId = "1",
                   name = "testProd1",
                   quantity = 2,
                   unitPrice = new Decimal(1.00)
               },
               new lineItemType
               {
                   itemId = "2",
                   name = "testProd2",
                   quantity = 3,
                   unitPrice = new Decimal(3.00)
               }
            };

            var TransactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                amount = 11.00m,
                payment = PaymentType,
                billTo = Address,
                lineItems = LineItems
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
                        Console.WriteLine("Successfully created transaction with Transaction ID: " +
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
    }
}

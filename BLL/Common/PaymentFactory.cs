using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public class PaymentFactory : IPaymentFactory
    {

        public  IPaymentProcess GetPaymentStrategy(PaymentMethod method)
        {
            switch (method)
            {

                case PaymentMethod.Cash:
                        return new CashPayment();
                   
                    

                default:
                    throw new Exception("Invalid Payment Method");
            }
        }
    }
}

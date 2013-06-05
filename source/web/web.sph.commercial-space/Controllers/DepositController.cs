﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class DepositController : Controller
    {
        public async Task<Deposit> Save(int id, IEnumerable<DepositPayment> deposits )
        {
            var context = new SphDataContext();
            var deposit = await context.LoadOneAsync<Deposit>(d => d.DepositId == id);

            deposit.DepositPaymentCollection.AddRange(deposits);

            using (var session = context.OpenSession())
            {
                session.Attach(deposit);
                await session.SubmitChanges();
            }
    
            return deposit;

            
        }

    }
}

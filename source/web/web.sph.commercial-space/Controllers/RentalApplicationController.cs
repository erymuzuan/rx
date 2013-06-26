﻿using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using WebGrease.Css.Extensions;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class RentalApplicationController : Controller
    {
        public async Task<ActionResult> Submit(RentalApplication rentalApplication)
        {
            await Task.Delay(2500);
            var context = new SphDataContext();
            var c = await context.LoadOneAsync<CommercialSpace>(cs => cs.CommercialSpaceId == rentalApplication.CommercialSpaceId);
            rentalApplication.Status = "New";
            rentalApplication.ApplicationDate = DateTime.Now;
            rentalApplication.CommercialSpace = c;

            var audit = new AuditTrail
                {
                    Operation = "Submit",
                    DateTime = DateTime.Now,
                    User = User.Identity.Name,
                    Type = typeof(RentalApplication).Name,
                    EntityId = rentalApplication.RentalApplicationId,
                    Note = "Permohonan melalui web"
                };

            using (var session = context.OpenSession())
            {
                session.Attach(rentalApplication);
                await session.SubmitChanges();
            }

            using (var session = context.OpenSession())
            {
                audit.EntityId = rentalApplication.RentalApplicationId;
                rentalApplication.RegistrationNo = string.Format("{0:yyyy}{1}", DateTime.Today,
                                                                 rentalApplication.RentalApplicationId.PadLeft());
                session.Attach(audit, rentalApplication);
                await session.SubmitChanges();
            }

            return Json(new { status = "success", registrationNo = rentalApplication.RegistrationNo });
        }
        public async Task<ActionResult> WaitingList(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Waiting";
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }

            return Json(true);
        }
        
        public async Task<ActionResult> Approved(int id)
        {
            var context = new SphDataContext();
            string message;
            bool result;
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            var existingApprovedCs =
                await context.GetCountAsync<RentalApplication>(r => r.CommercialSpaceId == dbItem.CommercialSpaceId && r.Status == "Approved" || r.Status == "Completed");
            if (existingApprovedCs == 0)
            {
                dbItem.Status = "Approved";
                message = "Approved";
                result = true;
            }
            else
            {
                message = "Ruang komersil sudah ada penyewa";
                result = false;
            }
            var audit = new AuditTrail
            {
                Operation = "Approval",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id
            };
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            return Json(new {message, result});
        }
        public async Task<ActionResult> Unsuccess(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Unsuccess";



            var audit = new AuditTrail
            {
                Operation = "Set application to not succeed",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id
            };

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            return Json(true);
        }
        public async Task<ActionResult> Returned(int id, ObjectCollection<Attachment> attachments)
        {
            await Task.Delay(5000);
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            if (null != attachments) dbItem.AttachmentCollection.ClearAndAddRange(attachments);

            var audit = new AuditTrail
            {
                Operation = "Returned rental application",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id,
                Note = "-"
            };

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            return Json(dbItem.RentalApplicationId);
        }
        public async Task<ActionResult> SendReturnedEmail(int id, ObjectCollection<Attachment> attachments, string remarks)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Returned";


            var audit = new AuditTrail
            {
                Operation = "Email retured notice",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id,
                Note = remarks
            };

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            const string bodyTemplate = "Sila lengkapkan dokumen berkenaan.";

            string emailBody = string.Format(bodyTemplate);
            var emailSubject = string.Format("Dokumen tidak lengkap");


            var emailMessage = new EmailMessage
            {
                Body = emailBody,
                To = new[] { dbItem.Contact.Email },
                From = "support@sph.gov.my",
                Subject = emailSubject
            };
            var channel = ObjectBuilder.GetObject<INotificationService>();
            channel.NotificationChannelCollection.ForEach(c => c.Send(emailMessage));

            return Json(dbItem.RentalApplicationId);
        }
        public async Task<ActionResult> GenerateReturnedLetter(int id, ObjectCollection<Attachment> attachments, string remarks)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Returned";


            var audit = new AuditTrail
            {
                Operation = "Generate retured letter",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id,
                Note = remarks
            };

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            var template = await context.GetScalarAsync<Setting, string>(s => s.Key == "Template.Returned.Letter",
                                                                   s => s.Value);
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var file = await store.GetContentAsync(template);
            var temp = System.IO.Path.GetTempFileName() + ".docx";
            System.IO.File.WriteAllBytes(temp, file.Content);
            var word = ObjectBuilder.GetObject<IDocumentGenerator>();
            Session["DocumentPath"] = temp;
            Session["DocumentTitle"] = string.Format("{0}-{1:yyyyMMdd}.Surat kembali.docx", dbItem.RegistrationNo, DateTime.Today);


            word.Generate(temp, dbItem, audit);

            return Json("");
        }
        public ActionResult Download()
        {
            var temp = Session["DocumentPath"] as string;
            var file = Session["DocumentTitle"] as string;
            if (string.IsNullOrWhiteSpace(temp)) return Content("");

            var content = System.IO.File.ReadAllBytes(temp);
            var type = MimeMapping.GetMimeMapping(".docx");
            return File(content, type, file);
        }
        public async Task<ActionResult> SaveOffer(int id, Offer offer)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Offer = offer;

            var audit = new AuditTrail
            {
                Operation = "Penyediaan tawaran",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id,
                Note = "-"
            };

            var deposit = await CreateOrUpdateDeposit(dbItem);
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit, deposit);
                await session.SubmitChanges();
            }
            return Json("");
        }
        public async Task<ActionResult> GenerateOfferLetter(int id)
        {
          
            var context = new SphDataContext();
            var rentalApplication = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            
            var audit = new AuditTrail
            {
                Operation = "Keluarkan surat tawaran",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id,
                Note = "-"
            };

            using (var session = context.OpenSession())
            {
                session.Attach(audit);
                await session.SubmitChanges();
            }

            var template = await context.GetScalarAsync<Setting, string>(s => s.Key == "Template.Offer.Letter",
                                                                   s => s.Value);
            var cs = await context.LoadOneAsync<CommercialSpace>(c => c.CommercialSpaceId == rentalApplication.Offer.CommercialSpaceId);

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var file = await store.GetContentAsync(template);
            var output = System.IO.Path.GetTempFileName() + ".docx";
            System.IO.File.WriteAllBytes(output, file.Content);
            var word = ObjectBuilder.GetObject<IDocumentGenerator>();
            Session["DocumentPath"] = output;
            Session["DocumentTitle"] = string.Format("{0}-{1:yyyyMMdd}.Surat tawaran.docx", rentalApplication.RegistrationNo, DateTime.Today);


            word.Generate(output, rentalApplication, audit, cs);

            return Json("");
        }
        public async Task<ActionResult> Declined(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Declined";



            var audit = new AuditTrail
            {
                Operation = "Declined",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id
            };

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            return Json(true);
        }
        public async Task<ActionResult> GenerateDeclinedLetter(int id)
        {
            const string status = "Declined";
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = status;


            var audit = new AuditTrail
            {
                Operation = "Keluarkan surat tawaran",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id,
                Note = "-"
            };

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            var template = await context.GetScalarAsync<Setting, string>(s => s.Key == "Template.Returned.Letter",
                                                                   s => s.Value);
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var file = await store.GetContentAsync(template);
            var temp = System.IO.Path.GetTempFileName() + ".docx";
            System.IO.File.WriteAllBytes(temp, file.Content);
            var word = ObjectBuilder.GetObject<IDocumentGenerator>();
            Session["DocumentPath"] = temp;
            Session["DocumentTitle"] = string.Format("{0}-{1:yyyyMMdd}.Surat penolakan.docx", dbItem.RegistrationNo, DateTime.Today);


            word.Generate(temp, dbItem, audit);

            return Json("");
        }
        public async Task<ActionResult> Complete(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Completed";


            var audit = new AuditTrail
            {
                Operation = "application flow completed",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id
            };
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            return Json(true);
        }
        private static async Task<Deposit> CreateOrUpdateDeposit(RentalApplication rental)
        {
            var context = new SphDataContext();
            var deposit = await context.LoadOneAsync<Deposit>(d => d.RegistrationNo == rental.RegistrationNo) ?? new Deposit();
            if (0 == deposit.DepositId)
            {
                deposit.DateTime = DateTime.Now;
                deposit.Name = rental.CompanyName ?? rental.Contact.Name;
                deposit.IDNumber = rental.CompanyRegistrationNo ?? rental.Contact.IcNo;
                deposit.RegistrationNo = rental.RegistrationNo;
                deposit.DueDate = rental.Offer.ExpiryDate;
                deposit.Amount = rental.Offer.Deposit;
            }
            else
            {
                deposit.Amount = rental.Offer.Deposit;
                deposit.DueDate = rental.Offer.ExpiryDate;
            }
            return deposit;
        }
        public async Task<ActionResult> SaveDepositPayment(int id, Offer offer)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Offer = offer;

            var audit = new AuditTrail
            {
                Operation = "Kemaskini pembayaran deposit",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id
            };

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            return Json(true);
        }
    }
}

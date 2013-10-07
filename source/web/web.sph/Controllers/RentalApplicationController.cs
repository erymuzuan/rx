using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using System.Linq;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize]
    public class RentalApplicationController : Controller
    {
        [AllowAnonymous]
        public async Task<ActionResult> Check(string id,string registrationNo)
        {
            var context = new SphDataContext();
            var query = context.RentalApplications.Where(r => r.RegistrationNo == registrationNo && r.Contact.IcNo == id);
            var lo = await context.LoadAsync(query);


            if (Request.ContentType.Contains("application/json"))
            {
                var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(JsonConvert.SerializeObject(lo.ItemCollection,settings));
            }

            return View(lo.ItemCollection);
        }

        public async Task<ActionResult> Print(int id)
        {
            var context = new SphDataContext();
            var app = await context.LoadOneAsync<RentalApplication>(a => a.RentalApplicationId == id);
            const string printSession = "print.rental.application";
            if (null == app && this.Request.HttpMethod == "POST")
            {
                // get this from the post
                var json = this.GetRequestBody();
                app = JsonConvert.DeserializeObject<RentalApplication>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                this.Session[printSession] = app;
            }
            if (null == app && this.Request.HttpMethod == "GET")
            {
                app = this.Session[printSession] as RentalApplication;
            }


            if (Request.ContentType.StartsWith("application/json"))
            {
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(await JsonConvert.SerializeObjectAsync(true));
            }

            return View(app);

        }
        
        [AllowAnonymous]
        public async Task<ActionResult> Submit(RentalApplication rentalapplication)
        {
            var context = new SphDataContext();
            var c = await context.LoadOneAsync<Space>(cs => cs.SpaceId == rentalapplication.SpaceId);
            rentalapplication.Status = "Baru";
            rentalapplication.ApplicationDate = DateTime.Now;
            rentalapplication.Space = c;

            var audit = new AuditTrail
                {
                    Operation = "Hantar",
                    DateTime = DateTime.Now,
                    User = User.Identity.Name,
                    Type = typeof(RentalApplication).Name,
                    EntityId = rentalapplication.RentalApplicationId,
                    Note = "Permohonan melalui web"
                };

            using (var session = context.OpenSession())
            {
                session.Attach(rentalapplication);
                await session.SubmitChanges();
            }

            using (var session = context.OpenSession())
            {
                audit.EntityId = rentalapplication.RentalApplicationId;
                rentalapplication.RegistrationNo = string.Format("{0:yyyy}{1}", DateTime.Today,
                                                                 rentalapplication.RentalApplicationId.PadLeft());
                session.Attach(audit, rentalapplication);
                await session.SubmitChanges();
            }

            return Json(new { status = "success", registrationNo = rentalapplication.RegistrationNo });
        }
        public async Task<ActionResult> WaitingList(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Menunggu";
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
            if (null == dbItem) return Json(new { status = "ERROR", message = "cannot find rental application with id " + id });

            var spaceApproved = await context.GetAnyAsync<RentalApplication>(r => r.SpaceId == dbItem.SpaceId && (r.Status == "Diluluskan" || r.Status == "Selesai"));
            if (!spaceApproved)
            {
                dbItem.Status = "Diluluskan";
                message = "Diluluskan";
                result = true;
            }
            else
            {
                message = "Ruang komersil sudah ada penyewa";
                result = false;
            }
            var audit = new AuditTrail
            {
                Operation = "Kelulusan Permohonan",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id
            };
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges("Approve");
            }

            return Json(new { message, result });
        }
        public async Task<ActionResult> Unsuccess(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Tidak Berjaya";



            var audit = new AuditTrail
            {
                Operation = "Permohonan Tidak Berjaya",
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
            await Task.Delay(2000);
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            if (null != attachments) dbItem.AttachmentCollection.ClearAndAddRange(attachments);

            var audit = new AuditTrail
            {
                Operation = "Permohonan Dikembalikan",
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

        public async Task<ActionResult> Update(int id, ObjectCollection<Attachment> attachments, string note)
        {
            await Task.Delay(5000);
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Remarks = note;
            if (null != attachments) dbItem.AttachmentCollection.ClearAndAddRange(attachments);

            var audit = new AuditTrail
            {
                Operation = "Permohonan Dikembalikan",
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
            dbItem.Status = "Dikembalikan";


            var audit = new AuditTrail
            {
                Operation = "Permohonan Dikembalikan melalui emel",
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
            channel.NotificationChannelCollection
                .ToList()
                .ForEach(c => c.Send(emailMessage));

            return Json(dbItem.RentalApplicationId);
        }
        public async Task<ActionResult> GenerateReturnedLetter(int id, ObjectCollection<Attachment> attachments, string remarks)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Dikembalikan";


            var audit = new AuditTrail
            {
                Operation = "Cetak Surat Permohonan Dikembalikan",
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
                Operation = "Cetak surat tawaran",
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
            var cs = await context.LoadOneAsync<Space>(c => c.SpaceId == rentalApplication.Offer.SpaceId);

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
            dbItem.Status = "Ditolak";



            var audit = new AuditTrail
            {
                Operation = "Permohonan Ditolak",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).Name,
                EntityId = id
            };

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges("Deny");
            }

            return Json(true);
        }
        public async Task<ActionResult> GenerateDeclinedLetter(int id)
        {
            const string status = "Ditolak";
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
            dbItem.Status = "Selesai";


            var audit = new AuditTrail
            {
                Operation = "Proses permohonan selesai",
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

using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;

namespace Bespoke.Dev_2.Domain
{
   public class Permohonan : Entity
   {
   private int m_permohonanId;
   public int PermohonanId
   {
       get{ return m_permohonanId;}
       set{ m_permohonanId = value;}
   }
     
        public override string ToString()
        {
            return "Permohonan:" + NoDaftar;
        }//member:NoDaftar
          private System.String m_noDaftar;
   public System.String NoDaftar
   {
       get{ return m_noDaftar;}
       set{ m_noDaftar = value;}
   }

//member:TarikhDaftar
          private System.DateTime m_tarikhDaftar;
   public System.DateTime TarikhDaftar
   {
       get{ return m_tarikhDaftar;}
       set{ m_tarikhDaftar = value;}
   }

//member:LokasiDiPohon
          private System.String m_lokasiDiPohon;
   public System.String LokasiDiPohon
   {
       get{ return m_lokasiDiPohon;}
       set{ m_lokasiDiPohon = value;}
   }

//member:NamaPemohon
          private System.String m_namaPemohon;
   public System.String NamaPemohon
   {
       get{ return m_namaPemohon;}
       set{ m_namaPemohon = value;}
   }

//member:MyKad
          private System.String m_myKad;
   public System.String MyKad
   {
       get{ return m_myKad;}
       set{ m_myKad = value;}
   }

//member:Telefon
          private System.String m_telefon;
   public System.String Telefon
   {
       get{ return m_telefon;}
       set{ m_telefon = value;}
   }

//member:Email
          private System.String m_email;
   public System.String Email
   {
       get{ return m_email;}
       set{ m_email = value;}
   }

//member:GredJawatan
          private System.String m_gredJawatan;
   public System.String GredJawatan
   {
       get{ return m_gredJawatan;}
       set{ m_gredJawatan = value;}
   }

//member:TarikhLantikanJawatan
          private System.String m_tarikhLantikanJawatan;
   public System.String TarikhLantikanJawatan
   {
       get{ return m_tarikhLantikanJawatan;}
       set{ m_tarikhLantikanJawatan = value;}
   }

//member:NoGaji
          private System.String m_noGaji;
   public System.String NoGaji
   {
       get{ return m_noGaji;}
       set{ m_noGaji = value;}
   }

//member:JenisPerkhidmatan
          private System.String m_jenisPerkhidmatan;
   public System.String JenisPerkhidmatan
   {
       get{ return m_jenisPerkhidmatan;}
       set{ m_jenisPerkhidmatan = value;}
   }

//member:Kementerian
          private Kementerian m_kementerian = new Kementerian();
   public Kementerian Kementerian
   {
       get{ return m_kementerian;}
       set{ m_kementerian = value;}
   }

//member:Pinjaman
          private System.Boolean m_pinjaman;
   public System.Boolean Pinjaman
   {
       get{ return m_pinjaman;}
       set{ m_pinjaman = value;}
   }

//member:JenisPinjaman
          private System.String m_jenisPinjaman;
   public System.String JenisPinjaman
   {
       get{ return m_jenisPinjaman;}
       set{ m_jenisPinjaman = value;}
   }

//member:AlamatRumahPinjaman
          private AlamatRumahPinjaman m_alamatRumahPinjaman = new AlamatRumahPinjaman();
   public AlamatRumahPinjaman AlamatRumahPinjaman
   {
       get{ return m_alamatRumahPinjaman;}
       set{ m_alamatRumahPinjaman = value;}
   }

//member:Status
          private System.String m_status;
   public System.String Status
   {
       get{ return m_status;}
       set{ m_status = value;}
   }

//member:NoGiliran
          private System.Int32 m_noGiliran;
   public System.Int32 NoGiliran
   {
       get{ return m_noGiliran;}
       set{ m_noGiliran = value;}
   }

   }
//class:NoDaftar

//class:TarikhDaftar

//class:LokasiDiPohon

//class:NamaPemohon

//class:MyKad

//class:Telefon

//class:Email

//class:GredJawatan

//class:TarikhLantikanJawatan

//class:NoGaji

//class:JenisPerkhidmatan

//class:Kementerian
   public class Kementerian: DomainObject
   {
   private System.String m_nama;
   public System.String Nama
   {
       get{ return m_nama;}
       set{ m_nama = value;}
   }

   private System.String m_bahagian;
   public System.String Bahagian
   {
       get{ return m_bahagian;}
       set{ m_bahagian = value;}
   }

   private System.String m_alamatJabatan;
   public System.String AlamatJabatan
   {
       get{ return m_alamatJabatan;}
       set{ m_alamatJabatan = value;}
   }

   private System.String m_postkod;
   public System.String Postkod
   {
       get{ return m_postkod;}
       set{ m_postkod = value;}
   }

   private System.String m_bandar;
   public System.String Bandar
   {
       get{ return m_bandar;}
       set{ m_bandar = value;}
   }

   private System.String m_negeri;
   public System.String Negeri
   {
       get{ return m_negeri;}
       set{ m_negeri = value;}
   }

   }







//class:Pinjaman

//class:JenisPinjaman

//class:AlamatRumahPinjaman
   public class AlamatRumahPinjaman: DomainObject
   {
   private System.String m_alamat;
   public System.String Alamat
   {
       get{ return m_alamat;}
       set{ m_alamat = value;}
   }

   private System.String m_poskod;
   public System.String Poskod
   {
       get{ return m_poskod;}
       set{ m_poskod = value;}
   }

   private System.String m_bandar;
   public System.String Bandar
   {
       get{ return m_bandar;}
       set{ m_bandar = value;}
   }

   private System.String m_negeri;
   public System.String Negeri
   {
       get{ return m_negeri;}
       set{ m_negeri = value;}
   }

   }





//class:Status

//class:NoGiliran

public partial class PermohonanController : System.Web.Mvc.Controller
{
//exec:Search
       public async Task<System.Web.Mvc.ActionResult> Search()
       {

            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = "dev/permohonan/_search";

            using(var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, request);
                var content = response.Content as System.Net.Http.StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(await content.ReadAsStringAsync());
            }
                   }
//exec:Save
       public async Task<System.Web.Mvc.ActionResult> Save()
       {

            var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Permohonan>(this);
            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("save");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new {success = true, status="OK", id = item.PermohonanId});
       }
       public Permohonan Item{get;set;}
//exec:Mohon
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> Mohon()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Permohonan>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Permohonan");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "26cfd245-558d-4dfc-a5ea-7217c98594ef");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "09171262-1c97-48b3-83ca-4d552cea8241");
           item.NoDaftar = (System.String)setter1.Field.GetValue(rc);
           var setter2 = operation.SetterActionChildCollection.Single(a => a.WebId == "0b478f45-796a-4cb3-bec3-622494a34a45");
           item.Status = (System.String)setter2.Field.GetValue(rc);
           var setter3 = operation.SetterActionChildCollection.Single(a => a.WebId == "91b42471-7619-4ee1-b10c-ab2579e08c03");
           item.TarikhDaftar = (System.DateTime)setter3.Field.GetValue(rc);
           var setter4 = operation.SetterActionChildCollection.Single(a => a.WebId == "afb4935c-3f79-4e11-b40b-5dbb4a4c33d6");
           item.NoGiliran = (System.Int32)setter4.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Mohon");
            }
            return Json(new {success = true, status="OK", id = item.PermohonanId});
       }
//exec:Lulus
       [HttpPost]
       [Authorize]
       public async Task<System.Web.Mvc.ActionResult> Lulus()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Permohonan>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Permohonan");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "3c790bb4-7662-44bb-9590-3bbd0dd77629");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "2a5b3904-bbf5-48c2-b094-ec6746d13373");
           item.Status = (System.String)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Lulus");
            }
            return Json(new {success = true, status="OK", id = item.PermohonanId});
       }
//exec:MasukanDalamMenunggu
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> MasukanDalamMenunggu()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Permohonan>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Permohonan");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "071d5f31-a75b-4f43-9cb3-4df04d819c23");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "c7358d80-8d03-4d77-971c-fabff8c4b835");
           item.Status = (System.String)setter1.Field.GetValue(rc);
           var setter2 = operation.SetterActionChildCollection.Single(a => a.WebId == "4af94e93-c17d-4749-a9f7-337809ee4f33");
           item.NoGiliran = (System.Int32)setter2.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("MasukanDalamMenunggu");
            }
            return Json(new {success = true, status="OK", id = item.PermohonanId});
       }
//exec:Agih
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> Agih()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Permohonan>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Permohonan");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "6d042ab2-af4c-4b9c-b988-a031d6fa76e0");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "a370ee3e-a7eb-4bc9-867d-918d57b511e4");
           item.Status = (System.String)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("Agih");
            }
            return Json(new {success = true, status="OK", id = item.PermohonanId});
       }
//exec:BayarDeposit
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> BayarDeposit()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Permohonan>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Permohonan");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "c8b28614-e0d7-4bf5-a151-794d181c0953");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "66356f9e-840b-4085-be19-5516bace541d");
           item.Status = (System.String)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("BayarDeposit");
            }
            return Json(new {success = true, status="OK", id = item.PermohonanId});
       }
//exec:SerahKunci
       [HttpPost]
       public async Task<System.Web.Mvc.ActionResult> SerahKunci()
       {
           var context = new Bespoke.Sph.Domain.SphDataContext();
           var item = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestJson<Permohonan>(this);
           if(null == item) item = this.Item;
           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == "Permohonan");
           var brokenRules = new ObjectCollection<ValidationResult>();
           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});

           var operation = ed.EntityOperationCollection.Single(o => o.WebId == "97038090-d0cd-4dd1-a623-454c041331df");
           var rc = new RuleContext(item);
           var setter1 = operation.SetterActionChildCollection.Single(a => a.WebId == "db1a37dd-b3c1-4087-a2ae-a58c272c5a19");
           item.Status = (System.String)setter1.Field.GetValue(rc);
           
            using(var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges("SerahKunci");
            }
            return Json(new {success = true, status="OK", id = item.PermohonanId});
       }
//exec:Remove
       [HttpDelete]
       public async Task<System.Web.Mvc.ActionResult> Remove(int id)
       {

            var repos = ObjectBuilder.GetObject<IRepository<Permohonan>>();
            var item = await repos.LoadOneAsync(id);
            if(null == item)
                return new HttpNotFoundResult();

            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {
                session.Delete(item);
                await session.SubmitChanges("delete");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Json(new {success = true, status="OK", id = item.PermohonanId});
       }
//exec:Schemas
       public async Task<System.Web.Mvc.ActionResult> Schemas()
       {
           var context = new SphDataContext();
           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == "Permohonan");
           var script = await ed.GenerateCustomXsdJavascriptClassAsync();
           this.Response.ContentType = "application/javascript";
           return Content(script);
       }
}
}

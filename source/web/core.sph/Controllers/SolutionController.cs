﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("solution")]
    [Authorize(Roles = "developers")]
    public class SolutionController : BaseController
    {
        [Route("diagnostics")]
        public async Task<ActionResult> StartDiagnostics()
        {
            var context = new SphDataContext();

            // validate all entities
            // - record name
            // - security
            // - icon/image
            // - Operation, name, security, Setter Path
            var entities = context.LoadFromSources<EntityDefinition>().ToList();
            var entitiesDiagnostics = new ConcurrentDictionary<string, BuildValidationResult>();

            async Task DiagnosBuildError(EntityDefinition f)
            {
                var diagnostics = ObjectBuilder.GetObject<IDeveloperService>().BuildDiagnostics;
                var errors = new List<BuildDiagnostic>();
                foreach (var dg in diagnostics)
                {
                    var ves = await dg.ValidateErrorsAsync(f);
                    errors.AddRange(ves);
                }
                var br = new BuildValidationResult();
                br.Errors.AddRange(errors);
                br.Uri = $"entity.details/{f.Id}";
                entitiesDiagnostics.TryAdd(f.Name, br);
            }

            var tasks0 = entities.Select(DiagnosBuildError);
            await Task.WhenAll(tasks0);


            // validate all forms
            // - path
            // - route
            var forms = context.LoadFromSources<EntityForm>().ToList();
            var formsDiagnostics = new ConcurrentDictionary<string, BuildValidationResult>();

            Func<EntityForm, Task> fr = async f =>
            {
                f.BuildDiagnostics = ObjectBuilder.GetObject<IDeveloperService>().BuildDiagnostics;
                var t = entities.SingleOrDefault(x => x.Id == f.EntityDefinitionId);
                var result = await f.ValidateBuildAsync(t);
                result.Uri = $"entity.form.designer/{f.EntityDefinitionId}/{f.Id}";
                formsDiagnostics.TryAdd(f.Name, result);

            };
            var tasks = forms.Select(x => fr(x));
            await Task.WhenAll(tasks);


            // validate all views
            // - column path
            // - link , is correct route
            // - icon
            // - some suggestion to spelling
            // - route
            var views = context.LoadFromSources<EntityView>().ToList();
            var viewsDiagnostics = new ConcurrentDictionary<string, BuildValidationResult>();

            Func<EntityView, Task> vr = async v =>
            {
                v.BuildDiagnostics = ObjectBuilder.GetObject<IDeveloperService>().BuildDiagnostics;
                var t = entities.SingleOrDefault(x => x.Id == v.EntityDefinitionId);
                var result = await v.ValidateBuildAsync(t);
                result.Uri = $"entity.view.designer/{v.EntityDefinitionId}/{v.Id}";
                viewsDiagnostics.TryAdd(v.Name, result);

            };
            var tasks2 = views.Select(vr);
            await Task.WhenAll(tasks2);


            // triggers
            // -> action
            // - filter
            // - compilation
            // - deployment
            // - active/inactive

            // workflow

            // 

            return Json(new { formsDiagnostics, entitiesDiagnostics, viewsDiagnostics });
        }
    }
}
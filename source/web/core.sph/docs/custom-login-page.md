#Creates your own brand login page

Since Reactive Developer doesnot dictate on how your security is handled, it's all up to the developers responsibility to create their own security mecanishm that follows ASP.Net security pipe line.


## Custom Login Page with ASP.Net MVC
You can easily create your own branded with ASP.Net MVC/Razor View


Creates a new Controller in App_Code folder, name this controller something like `LoginController`.

```csharp

using System.Web.Mvc;

namespace web.sph.App_Code
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}


```



Now create the corresponsing Razor View in Views\Login\Index.cshtml, your view need to contains at least a form with UserName and password field and set to POST to `Sph/SphAccount/Login` to use Reactive Developer internal ASP.Net membership providers. Or you can post it to your own custom membership provider.

```html
  <form method="POST" action="/Sph/SphAccount/Login" accept-charset="UTF-8">
	<input autofocus="autofocus" class="form-control" id="UserName" name="UserName" placeholder="UserName" required="True" type="text" value="" />
	<span class="field-validation-valid" data-valmsg-for="UserName" data-valmsg-replace="true"></span>
	<label class="controls"></label>
	<input class="form-control" id="Password" name="Password" placeholder="Password" required="True" type="password" />
	<span class="field-validation-valid" data-valmsg-for="Password" data-valmsg-replace="true"></span>
	<label class="checkbox">
		<input type="checkbox" name="remember" value="1">
		Remember me
	</label>

	<input type="hidden" value="" name="ReturnUrl"/>
	<button type="submit" name="submit" class="btn btn-default">Login</button>
</form>

```

The final things you need to do it to let the runtime knwo that about your new Login page, go to `web.config` file now locate this section ins `system.web`
```xml
<authentication mode="Forms">
    <forms loginUrl="~/Sph/SphAccount/Login" timeout="2880" name=".rx-developer.dev" />
</authentication>
```
to
```xml
<authentication mode="Forms">
    <forms loginUrl="~/Login" timeout="2880" name=".rx-developer.dev" />
</authentication>
```

Change the loginUrl to point to your new page
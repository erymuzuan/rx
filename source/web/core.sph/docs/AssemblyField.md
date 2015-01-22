#AssemblyField
##Overview
Allows you to execute a custom method in your own assembly. 

## Thing to know
If your method is an async method, you must return Task&lt;object>. 

Since the Field.GetValue is a synchronous method, it will wrap inside `Wait` and `ContinueWith`, so problem might arise with the thread waiting for the await. So make sure any async call is wrap with `ConfigureAwait(false)`. 
The word of warning : You might run into deadlock issue in this situation

Here is simple example of C# class
<pre>
using System;

namespace Bespoke
{
	public class Util
	{
		public string GetLookupValue(string value)
		{
            // may read it from database somewhere
			return value + "01 is the lookup for " + value;
		}
	}

}
</pre>

then compile it using csc.exe
<pre>
csc /t:library Util.cs
</pre>

![Field setting](http://i.imgur.com/37yZLKh.png)


## Using custom assembly that refer to your Entity
Not all you do is a simple lookup function with a simple type arguement, there may be time that you need to include the full object to your type.

<pre>
using System;
namespace Bespoke
{
	public class Util
	{
		public string GetOldRecordName(Bespoke.Dev_2002.Domain.Patient patient)
		{
            // may read it from database somewhere
			return patient.Mrn + "_old";
		}
	}

}
</pre>

Now compile your dll using this switch
<pre>
csc /t:library /r:c:\his\output\Dev.Patient.dll /r:c:\his\subscribers\domain.sph.dll .\code.cs
</pre>

Assuming that your project name is `Dev` and it sits in `c:\his`. Change the namespace accordingly.

To get the full power of C# editor we recommend you to download  [Visual Studio 2013 Express](http://go.microsoft.com/?linkid=9832222&clcid=0x409), that allows you to create a library project , get it.

Other alternative are
*  [SharpDevelop](http://www.icsharpcode.net/OpenSource/SD/Download/)
*  [MonoDevelop](http://monodevelop.com/)
##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>Location</td><td> - Assembly path</td></tr>
<tr><td>TypeName</td><td> - The name of the class, FullName</td></tr>
<tr><td>Method</td><td> - The method name</td></tr>
<tr><td>IsAsync</td><td> - Use async or not</td></tr>
<tr><td>AsyncTimeout</td><td> - Set the timeout for async call</td></tr>
<tr><td>MethodArgCollection</td><td> - The method arguements</td></tr>
</tbody></table>



## See also

[Field](Field.html)
[MethodArg](MethodArg.html)
[AssemblyField](AssemblyField.html)
[FunctionField](FunctionField.html)
[ConstantField](ConstantField.html)
[DocumentField](DocumentField.html)
[PropertyChangedField](PropertyChangedField.html)
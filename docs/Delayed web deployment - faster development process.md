##Why
Every time you Publish an asset, those that compiled into dll, in which there are later copied to web\bin. These includes but not limited to

* EntityDefinition
* TransformDefinition
* Adapters
* Workflow
* Trigger

it will automatically copied over to web\bin on every successful Publish. So when ASP.Net worker process detect there's change in your web\bin, it will restart to load the new assets. This is what you need to use any of the Controller in your asset immediately, but most of the time, you just want to publish and continue working on something else. Having your web server restart is one of the pain point we are trying to address in build 10307.


## How
What you will get on this build is a in memory-broker, this broker host a websocket server that constantly telling your developers panel what's going on. Including any changes in the output folder. Remember that any changes in output folder will not be automatically copied to web\bin.

To be able to use your controller or your assets, you will have to deploy your dll from the output. To do this, use the deploy buttons

![](http://www.reactivedeveloper.com/images/20150620.1019.developers.panel.compiler.output.png)

What it will do copy all your dll and related pdb files to web\bin, in this case it will force your development server to restart and make your assets available.

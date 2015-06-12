#Code snippets

Code snippets allow you to use some kind of template to write you code, if you commonly use code, you don't need to copy and paste them from clipboard everytime you need them. Wit code snippet you can just type CTRL + [SPACE] and select the one you are interested in and press [ENTER], then use [TAB] to replace the variables.

In this example , how to use code snippet to show your dialog

![](http://i.imgur.com/ZfoFL5G.png)


Type `di` and CTRL + [SPACE], then [ENTER] 

![](http://i.imgur.com/YXSQ2q4.png)

use [TAB] to go through your variable

## Creates your own code snippet
Go to your [Dev Home](/sph#de.home), you can find the link for [Code Snippet](/sph#snippets.dialog), Select the language for your snippet, In this example we are going to do csharp


Click the <i class="fa fa-plus"></i>, on title write `ob`, and the Note `Code snippet to call ObjectBuilder`, then on the Code
<pre>

dynamic ${1:dependency} = ObjectBuilder.GetObject("${2:key}");
</pre>


Now save it, and go to your code editor, you can type `ob` and CTRL + [SPACE] to use your snippet, over and over again
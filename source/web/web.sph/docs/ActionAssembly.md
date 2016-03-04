# Custom assembly trigger Action

With `Assembly` action you can execute your own custom code compiled into a .Net dll.

![](https://lh3.googleusercontent.com/-UyRZXSjI-o8/VtobW7aZlCI/AAAAAAAA5uU/kWA1_vLcnTM/s2048-Ic42/%25255BUNSET%25255D.png)

1. Name your Action
2. Select the assembly currently loaded, this is taken from `web\bin` and `subscribers` directory
3. Choose a type from your dll
4. Choose a method in the chosen type
5. Supply the parameters value for your method

**NOTE** : You assembly need to present in the `subscribers` folder and it will be loaded into the main the workers current `AppDomain`.

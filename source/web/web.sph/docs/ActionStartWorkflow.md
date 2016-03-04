# Starts Workflow Action

You can starts a custom `Workflow` as part of your `Trigger` action, this open up all sort of possibilities to what you can do, once your `Trigger` is fired.

![](https://lh3.googleusercontent.com/-5zQnPToWZfA/VtoXEMDgQEI/AAAAAAAA5uA/fEpCkO03jt0/s2048-Ic42/%25255BUNSET%25255D.png)

1. Select your `WorkflowDefinition`
2. Specify the version
3. Set the start up value to your `WorkflowDefinition` `Variable` value

![](https://lh3.googleusercontent.com/-JhFqcF_JiJ8/VtoX9bZYaSI/AAAAAAAA5uI/P9Ma_baOWec/s2048-Ic42/%25255BUNSET%25255D.png)

In this simple `WorkflowDefinition`, where you have a variable of type `Patient`, and you want make the current item from your Trigger to be the value of the variable. All you have to do is,

*  add a variable mapping
* Choose `Document` field, to extract a value from the current item
* Use `.` as your path
* Name it `Item` or anything you prefer

![](https://photos-2.dropbox.com/t/2/AABziL4Cehe3KfrMdotzQcQY7BIxQdchN044a3dVQA0pKQ/12/72555618/png/32x32/3/1457150400/0/2/2016-03-05%2007_22_15-Setting%20Trigger%20_%20Engineering%20Team%20Development.png/ELm4_4oEGAIgAigC/9MvnYKOV0AS_IPKDo8YfmUVlXETOarFHFYj4RZAtync?size_mode=5&size=32x32)

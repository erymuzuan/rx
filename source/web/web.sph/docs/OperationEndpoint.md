# Operation Endpoint

`REST` architectural design guide lines allows HTTP verbs like `POST`, `PUT`, `PATCH` and `DELETE` to be used for **unsafe** request.

RX Developer allows you to easily creates these HTTP endpoints to send action for your resources.

## PUT and POST
The HTTP methods POST and PUT aren't the HTTP equivalent of the CRUD's create and update. They both serve a different purpose. It's quite possible, valid and even preferred in some occasions, to use PUT to create resources, or use POST to update resources.

Use PUT when you can update a resource completely through a specific resource. For instance, if you know that a book resides at http://rx-sample.org/books/abc123, you can PUT a new resource representation of this book directly through a PUT on this URL.

If you do not know the actual resource location, for instance, when you add a new book, but do not have any idea where to store it, you can POST it to an URL, and let the server decide the actual URL.

```curl
PUT /books/abc321 HTTP/1.1
{
  "title": "Another day",
  "price":{
    "currency" : "USD",
    "value": 14.60
  }
}
```

```
POST /books HTTP/1.1
{
  "title": "Some day",
  "price":{
    "currency": "USD",
    "value": 25.00
  }
}

HTTP/1.1 201 Created
Location: /books/abc123
```


As soon as you know the new resource location, you can use PUT again to do updates to the Dooms day book. But as said before: you CAN add new resources through PUT as well. The next example is perfectly valid if your API provides this functionality:
```
PUT /articles/dooms-day HTTP/1.1
{
  "title" :"Dooms day",
  "price":{
    "currency": "USD",
    "value": 25.00
  }
}

HTTP/1.1 201 Created
Location: /books/dooms-day
```

Here, the client decided on the actual resource URL.

## How to create a POST Operation

Go to your `EntityDefinition` and select `endpoints` tab, you click on the `Add Operation endpoint` button
![](https://lh3.googleusercontent.com/-wja268cSbZQ/VuX4q9YKxtI/AAAAAAAA5u0/E-oaCIoQGaYVX2BhqPIE4XvLoABXGLyIwCCo/s2048-Ic42/%255BUNSET%255D)

A dialog will be shown for you
![](https://lh3.googleusercontent.com/-QVdBIZqEAQ0/VuX48vCpJBI/AAAAAAAA5u4/X5qjQAKV-h01aXbXyKXxwa5HeNdYPLRVwCCo/s2048-Ic42/%255BUNSET%255D)

1. Give your endpoint a name, so you could easily identify it later on
2. Select the EntityDefinition
3. Any note


![](https://i.imgur.com/wj6Bh9F.png)
1. Select HTTP POST
2. Conflict detection may not be application for HTTP POST, since POST will always creates a new resource. Conflict will only occur when you are updating existing resource which might have been outdated
3. Set the fields default value with `Setter`. Your client might not supply the value to your property in the message body, or you just want to make sure that s property will have the value you set.
4. The name of the operation
5. `Route`, in this example it's empty so that the url for this operation is `https://your-server-name/api/your-resources-name/`
6. `Resources` to identify your resource names, normally it will take the pluralize of your `EntityDefinition` name, and lower case it.

## Applying business rules
You might want to run some validation and business rules against the `POST` message that you just created. Go to your `business rules` tab, and select the rules you want. These rules will be evaluated and if any of the rules fails, HTTP status code of 422 will be returned to the client.

## Testing your endpoint
Once it's publish, the compiled output will be in your output folder, the dll will be named like `MyApp.EntityOperation.MyOperationName`, now you can deploy it, by click on the deploy tab in Developer Log, or copy it manually to your `web\bin` folder.

Now you can use curl command to execute your request, click on the curl command toolbar(1) and copy the code(2)

![](https://i.imgur.com/ennw0qw.png)

Open your [POSTMAN](https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop?hl=en) and click on the import button (1), from the pop dialog, select Raw Text(2) and click import(3)

![](https://i.imgur.com/Ea5LaNB.png)

No go to body tab, and you can edit your message payload

![](https://i.imgur.com/7K3bWu9.png)
